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
    
    public bool[] isInSkill = new bool[3];

    public bool[] isInMainGem = new bool[3];
    public List<bool> isInSupportGem1 = new List<bool>();
    public List<bool> isInSupportGem2 = new List<bool>();
    public List<bool> isInSupportGem3 = new List<bool>();
    
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
