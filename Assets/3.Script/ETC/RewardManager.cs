using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    private const int ARMOUR_COUNT = 1;
    private const int OPTION_COUNT = 24;

    public ItemTableManager itemTableManager;

    [SerializeField] private Inventory inventoryTab;

    [SerializeField] private GameObject dropedItem;
    [SerializeField] private Transform dropedItemParent;
    [SerializeField] private GameObject dropedItemUI;
    [SerializeField] private RectTransform dropedItemUIParent;

    [SerializeField] private GameObject dropedGold;
    [SerializeField] private Transform dropedGoldParent;
    [SerializeField] private GameObject dropedGoldUI;
    [SerializeField] private RectTransform dropedGoldUIParent;

    private void Awake()
    {
        Instance = this;
    }

    public void DropItem(Vector3 itemPos)
    {
        var idx = Random.Range(0, 6);
        Item item = MakeEquipment(itemTableManager.ItemTable[idx]);

        var dropItem = Instantiate(dropedItem, itemPos + new Vector3(0, 0.5f, 0), Quaternion.identity,
            dropedItemParent);
        var dropItemUI = Instantiate(dropedItemUI, dropedItemUIParent);

        var dropItemSlot = dropItemUI.GetComponent<InventorySlot>();
        var dropItemRect = dropItemUI.GetComponent<RectTransform>();
        var dropItemText = dropItemUI.GetComponentInChildren<Text>();

        dropItemSlot.SetSlot(item);
        DropItemUI.Instance.SetUIText(dropItemText, item);
        DropItemUI.Instance.RegisterDrop(dropItem, dropItemRect); // ✅ 추가됨
    }

    public void DropGold(int min, int max, Vector3 itemPos)
    {
        var dropGold = Instantiate(dropedGold, itemPos + new Vector3(0, 0.5f, 0), Quaternion.identity,
            dropedGoldParent);
        var dropGoldUI = Instantiate(dropedGoldUI, dropedGoldUIParent);

        var dropGoldRect = dropGoldUI.GetComponent<RectTransform>();
        var dropGoldText = dropGoldUI.GetComponentInChildren<Text>();

        int goldAmount = Random.Range(min, max);

        dropGoldText.text = $"{goldAmount} gold";

        DropItemUI.Instance.RegisterGoldDrop(dropGold, dropGoldRect);

        if (dropGold.TryGetComponent(out DropGoldTrigger trigger))
        {
            trigger.Setup(dropGoldRect, goldAmount);
        }
    }

    public Item MakeEquipment(Item baseItem)
    {
        Item newItem = null;
        IEquipment equip = null;

        string[] tokens = baseItem.ItemData.Parameter.Split(',');

        switch (baseItem.ItemData.ItemType)
        {
            case "BodyArmour":
                newItem = new BodyArmour();
                break;
            case "Gloves":
                newItem = new Gloves();
                break;
            case "Helmet":
                newItem = new Helmet();
                break;
            case "Boots":
                newItem = new Boots();
                break;
            case "Weapon":
                if (int.Parse(tokens[0]) == 0)
                {
                    newItem = new OneHandSword();
                }
                else if (int.Parse(tokens[0]) == 1)
                {
                    newItem = new Wand();
                }
                break;
            default:
                Debug.LogError($"Unknown ItemType: {baseItem.ItemData.ItemType}");
                return null;
        }

        // 공통 세팅
        newItem.Sprite = baseItem.Sprite;
        newItem.DragSize = baseItem.DragSize;
        newItem.ItemData = baseItem.ItemData;

        equip = newItem as IEquipment;

        equip.Rarity = Random.Range(0, 3);

        if (equip is IWeapon weapon)
        {
            weapon.MinAttackDamage = int.Parse(tokens[1]);
            weapon.MaxAttackDamage = int.Parse(tokens[2]);
            weapon.MinSpellDamage = int.Parse(tokens[3]);
            weapon.MaxSpellDamage = int.Parse(tokens[4]);
            GenerateRandomOptions(weapon);
        }
        else if (equip is IArmour armor)
        {
            armor.Armor = Random.Range(int.Parse(tokens[0]), int.Parse(tokens[1]));
            armor.Evasion = Random.Range(int.Parse(tokens[2]), int.Parse(tokens[3]));
            GenerateRandomOptions(armor);
        }

        return newItem;
    }

    private void GenerateRandomOptions(IEquipment equip)
    {
        equip.OptionIdx.Clear();
        equip.CashingValue.Clear();

        int optionCount = GetOptionCount(equip.Rarity);

        for (int i = 0; i < optionCount; i++)
        {
            int selectedIndex = GetUniqueRandomOption(equip);

            int value = GenerateOptionValue(selectedIndex);
            equip.CashingValue.Add(selectedIndex, value);
            equip.OptionIdx.Add(selectedIndex);
            equip.DescriptionDic.Add(selectedIndex, GenerateDescription(selectedIndex, value));
        }
    }

    private int GetOptionCount(int rarity)
    {
        switch (rarity)
        {
            case 0:
                return 0;
            case 1:
                return Random.Range(1, 3);
            case 2:
                return Random.Range(2, 5);
        }

        return 0;
    }

    private int GetUniqueRandomOption(IEquipment equip)
    {
        int selected;

        do
        {
            selected = equip.PickRandomOption();
        } while (equip.OptionIdx.Contains(selected));

        return selected;
    }

    private int GenerateOptionValue(int index)
    {
        return index switch
        {
            0 => Random.Range(10, 21), // 최대 생명력 +#
            1 => Random.Range(1, 6), // 최대 생명력 +#%
            2 => Random.Range(1, 4), // 생명력 재생 속도 +#
            3 => Random.Range(1, 6), // 생명력 재생 속도 +#%
            4 => Random.Range(5, 16), // 최대 마나 +#
            5 => Random.Range(1, 6), // 최대 마나 +#%
            6 => Random.Range(1, 4), // 마나 재생 속도 +#
            7 => Random.Range(1, 6), // 마나 재생 속도 +#%
            8 => Random.Range(10, 21), // 방어력 +#
            9 => Random.Range(5, 11), // 방어력 +#%
            10 => Random.Range(10, 21), // 회피 +#
            11 => Random.Range(5, 11), // 회피 +#%
            12 => Random.Range(1, 4), // 힘
            13 => Random.Range(1, 4), // 민첩
            14 => Random.Range(1, 4), // 지능
            15 => Random.Range(5, 11), // 이동속도 +#%
            16 => Random.Range(5, 11), // 공격속도 +#%
            17 => Random.Range(5, 11), // 시전속도 +#%
            18 => Random.Range(5, 11), // 치명타 확률 +#%
            19 => Random.Range(10, 21), // 치명타 피해 +#%
            20 => Random.Range(3, 7), // 공격력 추가 +# ~ +#
            21 => Random.Range(5, 11), // 공격력 추가 +#%
            22 => Random.Range(3, 7), // 주문력 추가 +# ~ +#
            23 => Random.Range(5, 11), // 주문력 추가 +#%
            _ => 0 // 잘못된 인덱스
        };
    }

    private string GenerateDescription(int key, int value)
    {
        return key switch
        {
            0 => $"최대 생명력 +{value} 증가",
            1 => $"최대 생명력 +{value}% 증가",
            2 => $"생명력 재생 속도 +{value} 증가",
            3 => $"생명력 재생 속도 +{value}% 증가",
            4 => $"최대 마나 +{value} 증가",
            5 => $"최대 마나 +{value}% 증가",
            6 => $"마나 재생 속도 +{value} 증가",
            7 => $"마나 재생 속도 +{value}% 증가",
            8 => $"방어력 +{value} 증가",
            9 => $"방어력 +{value}% 증가",
            10 => $"회피 +{value} 증가",
            11 => $"회피 +{value}% 증가",
            12 => $"힘 +{value}",
            13 => $"민첩 +{value}",
            14 => $"지능 +{value}",
            15 => $"이동속도 +{value}% 증가",
            16 => $"공격속도 +{value}% 증가",
            17 => $"시전속도 +{value}% 증가",
            18 => $"치명타 확률 +{value}% 증가",
            19 => $"치명타 피해 +{value}% 증가",
            20 => $"공격력 +{value} 증가",
            21 => $"공격력 +{value}% 증가",
            22 => $"주문력 +{value} 증가",
            23 => $"주문력 +{value}% 증가",
            _ => $"알 수 없는 옵션 (key={key}, value={value})"
        };
    }
}