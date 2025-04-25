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

    private class OptionMeta
    {
        public Action<EquipmentStst, float> Apply;
        public Action<EquipmentStst, float> Remove;
    }

    private Dictionary<int, OptionMeta> optionTable = new();
    private Dictionary<int, string> DescriptionDic = new();

    [SerializeField] private PlayerStatPanelUI playerStatPanelUI;
    
    private void Awake()
    {
        Instance = this;

        EquipmentStat = new EquipmentStst();
        EquipmentStat.Initialize();

        InitOptionTable();
    }

    private void InitOptionTable()
{
    optionTable[0] = new OptionMeta
    {
        Apply = (s, v) => { s.Hp += v; },
        Remove = (s, v) => { s.Hp -= v; }
    };

    optionTable[1] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseHp += v / 100f; },
        Remove = (s, v) => { s.IncreaseHp -= v / 100f; }
    };

    optionTable[2] = new OptionMeta
    {
        Apply = (s, v) => { s.HpRegenRate += v; },
        Remove = (s, v) => { s.HpRegenRate -= v; }
    };

    optionTable[3] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseHpRegenRate += v / 100f; },
        Remove = (s, v) => { s.IncreaseHpRegenRate -= v / 100f; }
    };

    optionTable[4] = new OptionMeta
    {
        Apply = (s, v) => { s.Mp += v; },
        Remove = (s, v) => { s.Mp -= v; }
    };

    optionTable[5] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseMp += v / 100f; },
        Remove = (s, v) => { s.IncreaseMp -= v / 100f; }
    };

    optionTable[6] = new OptionMeta
    {
        Apply = (s, v) => { s.MpRegenRate += v; },
        Remove = (s, v) => { s.MpRegenRate -= v; }
    };

    optionTable[7] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseMpRegenRate += v / 100f; },
        Remove = (s, v) => { s.IncreaseMpRegenRate -= v / 100f; }
    };

    optionTable[8] = new OptionMeta
    {
        Apply = (s, v) => { s.Armour += v; },
        Remove = (s, v) => { s.Armour -= v; }
    };

    optionTable[9] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseArmour += v / 100f; },
        Remove = (s, v) => { s.IncreaseArmour -= v / 100f; }
    };

    optionTable[10] = new OptionMeta
    {
        Apply = (s, v) => { s.Evasion += v; },
        Remove = (s, v) => { s.Evasion -= v; }
    };

    optionTable[11] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseEvasion += v / 100f; },
        Remove = (s, v) => { s.IncreaseEvasion -= v / 100f; }
    };

    optionTable[12] = new OptionMeta
    {
        Apply = (s, v) => { s.Strength += (int)v; },
        Remove = (s, v) => { s.Strength -= (int)v; }
    };

    optionTable[13] = new OptionMeta
    {
        Apply = (s, v) => { s.Dexterity += (int)v; },
        Remove = (s, v) => { s.Dexterity -= (int)v; }
    };

    optionTable[14] = new OptionMeta
    {
        Apply = (s, v) => { s.Intelligence += (int)v; },
        Remove = (s, v) => { s.Intelligence -= (int)v; }
    };

    optionTable[15] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseMovementSpeed += v / 100f; },
        Remove = (s, v) => { s.IncreaseMovementSpeed -= v / 100f; }
    };

    optionTable[16] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseAttackSpeed += v / 100f; },
        Remove = (s, v) => { s.IncreaseAttackSpeed -= v / 100f; }
    };

    optionTable[17] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseCastSpeed += v / 100f; },
        Remove = (s, v) => { s.IncreaseCastSpeed -= v / 100f; }
    };

    optionTable[18] = new OptionMeta
    {
        Apply = (s, v) => { s.CriticalChance += v / 100f; },
        Remove = (s, v) => { s.CriticalChance -= v / 100f; }
    };

    optionTable[19] = new OptionMeta
    {
        Apply = (s, v) => { s.CriticalDamage += v / 100f; },
        Remove = (s, v) => { s.CriticalDamage -= v / 100f; }
    };

    optionTable[20] = new OptionMeta
    {
        Apply = (s, v) => { s.MinAttackDamage += (int)v; s.MaxAttackDamage += (int)v; },
        Remove = (s, v) => { s.MinAttackDamage -= (int)v; s.MaxAttackDamage -= (int)v;}
    };

    optionTable[21] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseAttackDamage += v / 100f; },
        Remove = (s, v) => { s.IncreaseAttackDamage -= v / 100f; }
    };

    optionTable[22] = new OptionMeta
    {
        Apply = (s, v) => { s.MinSpellDamage += (int)v; s.MaxSpellDamage += (int)v; },
        Remove = (s, v) => { s.MinSpellDamage -= (int)v; s.MaxSpellDamage -= (int)v; }
    };

    optionTable[23] = new OptionMeta
    {
        Apply = (s, v) => { s.IncreaseSpellDamage += v / 100f; },
        Remove = (s, v) => { s.IncreaseSpellDamage -= v / 100f; }
    };
}


    public void EquipEquipment(Item item)
    {
        if (item is not IEquipment equip) return;
        
        foreach (var kv in equip.CashingValue)
        {
            if (optionTable.TryGetValue(kv.Key, out var meta))
            {
                meta.Apply(EquipmentStat, kv.Value);
                //액션에서 추가하는 디스크립션이랑 여기랑 달라서 null이 뜸
            }
        }

        playerStatPanelUI.UpdateText();
    }

    public void UnEquipEquipment(Item item)
    {
        if (item is not IEquipment equip) return;

        foreach (var kv in equip.CashingValue)
        {
            if (optionTable.TryGetValue(kv.Key, out var meta))
            {
                meta.Remove(EquipmentStat, kv.Value);
            }
        }

        playerStatPanelUI.UpdateText();
    }
}
