using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using Random = UnityEngine.Random;

public class TempMakeWeapon : MonoBehaviour
{
    public InventorySlot inventorySlot;
    public InventorySlot inventorySlot1;

    public ItemTableManager itemTableManager;
    
    public IEnumerator Start()
    {
        yield return null;
        
        Item item = RewardManager.Instance.MakeEquipment(itemTableManager.ItemTable[8]);
        inventorySlot.SetSlot(item);
        
        item = RewardManager.Instance.MakeEquipment(itemTableManager.ItemTable[10]);
        inventorySlot1.SetSlot(item);
    }
}
