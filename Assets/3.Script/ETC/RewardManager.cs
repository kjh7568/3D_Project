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

        equip.Rarity = 1; // Random.Range(0, 3) 가능
        equip.Armor = UnityEngine.Random.Range(int.Parse(tokens[0]), int.Parse(tokens[1]));
        equip.Evasion = UnityEngine.Random.Range(int.Parse(tokens[2]), int.Parse(tokens[3]));

        GenerateRandomOptions(equip);

        return newItem;
    }

    public void GenerateRandomOptions(IEquipment equip)
    {
        equip.OptionIdx.Clear();

        if (equip.Rarity == 0)
            return;

        int optionCount = 0;

        if (equip.Rarity == 1)
            optionCount = 1; // Random.Range(1, 3) 가능
        else if (equip.Rarity == 2)
            optionCount = Random.Range(2, 5);

        for (int i = 0; i < optionCount; i++)
        {
            int selectedIndex = equip.PickRandomOption();

            if (equip.OptionIdx.Contains(selectedIndex))
            {
                i--;
                continue;
            }

            switch (selectedIndex)
            {
                case 0: // 체력 추가 옵션
                    int val = Random.Range(10, 21);
                    equip.CashingValue.Add(selectedIndex, val);
                    break;
            }

            equip.OptionIdx.Add(selectedIndex);
        }
    }
}