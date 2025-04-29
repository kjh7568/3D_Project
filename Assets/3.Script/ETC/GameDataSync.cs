using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSync : MonoBehaviour
{
    public static GameDataSync Instance;

    public PlayerStat playerStat = null;
    public EquipmentStst equipmentStat = null;

    public List<Item> inventorySlots = null;
    public List<Item> equipmentSlots = null;
    public List<Item> gemSlots = null;

    public int gold;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
