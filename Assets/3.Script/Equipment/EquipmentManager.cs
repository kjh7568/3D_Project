using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    
    public EquipmentStst EquipmentStat;
    private const int OPTION_COUNT = 24;
    
    public Action<Item>[] addOptionHandler = new Action<Item>[OPTION_COUNT];
    public Action<Item>[] removeOptionHandler = new Action<Item>[OPTION_COUNT];
    private void Awake()
    {
        Instance = this;
        
        EquipmentStat = new EquipmentStst();
        EquipmentStat.Initialize();
        
        addOptionHandler[0] = AddHp;
        removeOptionHandler[0] = RemoveHp;
    }

    public void EquipEquipment(Item item)
    {
        if (item is IEquipment equipment)
        {
            for (int i = 0; i < equipment.OptionIdx.Count; i++)
            {
                int optionIndex = equipment.OptionIdx[i];
                addOptionHandler[optionIndex]?.Invoke(item);
            }
        }
    }

    public void UnEquipEquipment(Item item)
    {
        if (item is IEquipment equipment)
        {
            for (int i = 0; i < equipment.OptionIdx.Count; i++)
            {
                int optionIndex = equipment.OptionIdx[i];
                removeOptionHandler[optionIndex]?.Invoke(item);
            }
        }
    }
    
    public void AddHp(Item item)
    {
        if (item is IEquipment equipment && equipment.CashingValue.ContainsKey(0))
        {
            EquipmentManager.Instance.EquipmentStat.Hp += equipment.CashingValue[0];
            Player.LocalPlayer.RealStat.UpdateStat();
        }
    }

    public void RemoveHp(Item item)
    {
        if (item is IEquipment equipment && equipment.CashingValue.ContainsKey(0))
        {
            EquipmentManager.Instance.EquipmentStat.Hp -= equipment.CashingValue[0];
            Player.LocalPlayer.RealStat.UpdateStat();
        }
    }
}
