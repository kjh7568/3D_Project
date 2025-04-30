using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        shopSlots[0].SetSlot(RewardManager.Instance.MakeEquipment(RewardManager.Instance.itemTableManager.ItemTable[4]));
        for (int i = 1; i < 11; i++)
        {
            var randIdx = PickIndex();
            Item item = RewardManager.Instance.MakeEquipment(
                RewardManager.Instance.itemTableManager.ItemTable[randIdx]);

            shopSlots[i].SetSlot(item);
        }
    }
    
    private int PickIndex()
    {
        int index = -1;
        
        float mainRoll = Random.Range(0f, 100f);
        if (mainRoll < 70f) // 70% 그룹
        {
            int subIndex = Random.Range(0, 6); // 0~5 (6개 항목)
            switch (subIndex)
            {
                case 0: index = 0; break; // 0
                case 1: index = 1; break; // 25
                case 2: index = 2; break; // 50
                case 3: index = 3; break; // 75
                case 4: // (100, 101)
                    float roll = Random.Range(0f, 100f);
                    index = (roll < 70f) ? 4 : 5; // 100(70%) or 101(30%)
                    break;
                case 5: // (150, 151)
                    float roll2 = Random.Range(0f, 100f);
                    index = (roll2 < 70f) ? 6 : 7; // 150(70%) or 151(30%)
                    break;
            }
        }
        else // 30% 그룹
        {
            float subGroupRoll = Random.Range(0f, 100f);
            if (subGroupRoll < 70f) // (200, 201)
            {
                index = (Random.Range(0, 2) == 0) ? 8 : 9; // 200 or 201
            }
            else // (300, 301, 302)
            {
                int roll = Random.Range(0, 3); // 0, 1, 2
                switch (roll)
                {
                    case 0: index = 10; break; // 300
                    case 1: index = 11; break; // 301
                    case 2: index = 12; break; // 302
                }
            }
        }

        return index;
    }
}