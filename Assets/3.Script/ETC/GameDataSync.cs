using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSync : MonoBehaviour
{
    public static GameDataSync Instance;

    public Inventory inventoryTab;
    public Inventory gemTab;

    public EquipmentManager syncEquipmentManager;
    public SkillManager syncSkillManager;
    
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
