using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardManager : MonoBehaviour
{
    private const int ARMOUR_COUNT = 1;
    private const int OPTION_COUNT = 24;

    [SerializeField] private ItemTableManager itemTableManager;
    [SerializeField] private Inventory inventoryTab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Item item = MakeEquipment(itemTableManager.ItemTable[Random.Range(0, 4)]);
            inventoryTab.AddItem(item);
        }
    }

    public Item MakeEquipment(Item baseItem)
    {
        Item newItem = null;
        IEquipment equip = null;

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
            default:
                Debug.LogError($"Unknown ItemType: {baseItem.ItemData.ItemType}");
                return null;
        }

        // 공통 세팅
        newItem.Sprite = baseItem.Sprite;
        newItem.DragSize = baseItem.DragSize;
        newItem.ItemData = baseItem.ItemData;

        equip = newItem as IEquipment;

        string[] tokens = newItem.ItemData.Parameter.Split(',');

        equip.Rarity = Random.Range(0, 3);
        equip.Armor = UnityEngine.Random.Range(int.Parse(tokens[0]), int.Parse(tokens[1]));
        equip.Evasion = UnityEngine.Random.Range(int.Parse(tokens[2]), int.Parse(tokens[3]));

        GenerateRandomOptions(equip);

        return newItem;
    }

    public void GenerateRandomOptions(IEquipment equip)
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
            0  => Random.Range(10, 21),  // 최대 생명력 +#
            1  => Random.Range(1, 6),    // 최대 생명력 +#%
            2  => Random.Range(1, 4),    // 생명력 재생 속도 +#
            3  => Random.Range(1, 6),    // 생명력 재생 속도 +#%
            4  => Random.Range(5, 16),   // 최대 마나 +#
            5  => Random.Range(1, 6),    // 최대 마나 +#%
            6  => Random.Range(1, 4),    // 마나 재생 속도 +#
            7  => Random.Range(1, 6),    // 마나 재생 속도 +#%
            8  => Random.Range(10, 21),  // 방어력 +#
            9  => Random.Range(5, 11),   // 방어력 +#%
            10 => Random.Range(10, 21),  // 회피 +#
            11 => Random.Range(5, 11),   // 회피 +#%
            12 => Random.Range(1, 4),    // 힘
            13 => Random.Range(1, 4),    // 민첩
            14 => Random.Range(1, 4),    // 지능
            15 => Random.Range(5, 11),   // 이동속도 +#%
            16 => Random.Range(5, 11),   // 공격속도 +#%
            17 => Random.Range(5, 11),   // 시전속도 +#%
            18 => Random.Range(5, 11),   // 치명타 확률 +#%
            19 => Random.Range(10, 21),  // 치명타 피해 +#%
            20 => Random.Range(3, 7),    // 공격력 추가 +# ~ +#
            21 => Random.Range(5, 11),   // 공격력 추가 +#%
            22 => Random.Range(3, 7),    // 주문력 추가 +# ~ +#
            23 => Random.Range(5, 11),   // 주문력 추가 +#%
            _  => 0 // 잘못된 인덱스
        };
    }
}