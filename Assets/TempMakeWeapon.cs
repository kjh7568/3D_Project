using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using Random = UnityEngine.Random;

public class TempMakeWeapon : MonoBehaviour
{
    public InventorySlot inventorySlot;

    public ItemTableManager itemTableManager;
    
    public IEnumerator Start()
    {
        yield return null;
        
        Item item = RewardManager.Instance.MakeEquipment(itemTableManager.ItemTable[4]);
        
        inventorySlot.SetSlot(item);
    }
}
