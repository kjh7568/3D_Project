using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopper : MonoBehaviour
{
    public InventorySlot[] shopSlots;
    public Text goldText;

    private void OnEnable()
    {
        if (Player.LocalPlayer != null) SetGoldText();
    }

    public void SetGoldText()
    {
        goldText.text = $"{Player.LocalPlayer.gold}";
    }
    
    private void Start()
    {
        for (int i = 1; i < 11; i++)
        {
            var randIdx = UnityEngine.Random.Range(0, 6);
            Item item = RewardManager.Instance.MakeEquipment(
                RewardManager.Instance.itemTableManager.ItemTable[randIdx]);

            shopSlots[i].SetSlot(item);
        }
    }
}