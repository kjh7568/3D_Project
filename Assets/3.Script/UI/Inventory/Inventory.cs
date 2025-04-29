using System;
using System.Collections;
using System.Collections.Generic;
using DungeonArchitect.Flow.Items;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //보통 게임에선 이 슬롯을 최대값으로 적용시켜놓고 lock을 걸어버리는 방법을 사용
    public string tabName;

    private InventorySlot[] slots;

    // Start is called before the first frame update
    public void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }

    public void Initialize(List<Item> items)
    {
        // for (int i = 0; i < slots.Length; i++)
        // {
        //     //임시 코드
        //     slots[i].SetSlot(null);
        // }

        if (items == null) return;

        for (int i = 0; i < items.Count; i++)
        {
            if (slots.Length <= i - 1) break;
            slots[i].SetSlot(items[i]);
        }
    }

    private void OnDestroy()
    {
        if (tabName.Equals("Inventory"))
        {
            GameDataSync.Instance.inventorySlots = new List<Item>();
            foreach (var slot in slots)
            {
                GameDataSync.Instance.inventorySlots.Add(slot.Item);
            }
        }
        else if (tabName.Equals("Equipment"))
        {
            GameDataSync.Instance.equipmentSlots = new List<Item>();
            foreach (var slot in slots)
            {
                GameDataSync.Instance.equipmentSlots.Add(slot.Item);
            }
        }
        else if (tabName.Equals("Gem"))
        {
            GameDataSync.Instance.gemSlots = new List<Item>();
            foreach (var slot in slots)
            {
                GameDataSync.Instance.gemSlots.Add(slot.Item);
            }
        }
    }

    public void AddItem(Item item)
    {
        BodyArmour armour = item as BodyArmour;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == null)
            {
                slots[i].SetSlot(item);
                break;
            }
        }
    }

    public bool IsIn(InventorySlot slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slot.Equals(slots[i]))
                return true;
        }

        return false;
    }
}