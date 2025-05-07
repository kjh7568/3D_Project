using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingInfomationUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Image itemRarity;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemBaseStat;

    [SerializeField] private RectTransform optionParents;
    [SerializeField] private GameObject optionPrefab;

    private List<GameObject> options = new List<GameObject>();
    private bool isAlreadyMake = false;
    private Item prevItem = new Item();

    public void SetItemBaseInfo(Item item)
    {
        if (prevItem.Equals(item))
        {
            return;
        }

        prevItem = item;

        if (item.DragSize.y < 200f)
        {
            itemSprite.gameObject.TryGetComponent(out RectTransform rectTransform);
            Vector2 size = rectTransform.sizeDelta;
            size.y = 280f;
            rectTransform.sizeDelta = size;
        }
        else
        {
            itemSprite.gameObject.TryGetComponent(out RectTransform rectTransform);
            Vector2 size = rectTransform.sizeDelta;
            size.y = 420f;
            rectTransform.sizeDelta = size;
        }

        itemSprite.sprite = item.Sprite;
        itemName.text = item.ItemData.Name;

        if (item is IEquipment equip)
        {
            SetItemRarity(equip);
            SetItemBaseStat(equip);
            MakeOptionText(equip);
        }
        else
        {
            MakeOptionText(item);
            SetItemBaseStat(item);
        }
    }

    private void SetItemRarity(IEquipment item)
    {
        if (item.Rarity == 0)
        {
            itemRarity.color = Color.white;
        }
        else if (item.Rarity == 1)
        {
            itemRarity.color = Color.blue;
        }
        else if (item.Rarity == 2)
        {
            itemRarity.color = Color.yellow;
        }
    }

    private void SetItemBaseStat(IEquipment item)
    {
        itemBaseStat.gameObject.SetActive(true);
        
        if (item is IWeapon weapon)
        {
            itemBaseStat.text = $"공격력: {weapon.MinAttackDamage} ~ {weapon.MaxAttackDamage} | 주문력: {weapon.MinSpellDamage} ~ {weapon.MaxSpellDamage - 1}";
        }
        else if (item is IArmour armor)
        {
            itemBaseStat.text = $"방어력: {armor.Armor} | 회피: {armor.Evasion}";
        }
    }

    private void SetItemBaseStat(Item item)
    {
        itemBaseStat.gameObject.SetActive(false);
    }

    private void MakeOptionText(IEquipment item)
    {
        foreach (var obj in options)
        {
            Destroy(obj);
        }

        options.Clear();

        for (int i = 0; i < item.DescriptionDic.Count; i++)
        {
            var obj = Instantiate(optionPrefab, optionParents);

            if (obj.TryGetComponent(out Text text))
            {
                text.text = item.DescriptionDic[item.OptionIdx[i]];
            }

            options.Add(obj);
        }
    }

    private void MakeOptionText(Item item)
    {
        foreach (var obj in options)
        {
            Destroy(obj);
        }

        options.Clear();

        var descriptionText = Instantiate(optionPrefab, optionParents);

        if (descriptionText.TryGetComponent(out Text text) &&
            descriptionText.TryGetComponent(out RectTransform rectTransform))
        {
            string[] token = item.ItemData.Parameter.Split('_');
            rectTransform.sizeDelta = new Vector2(400, 315);
            text.text = token[2];
        }

        options.Add(descriptionText);
    }
}