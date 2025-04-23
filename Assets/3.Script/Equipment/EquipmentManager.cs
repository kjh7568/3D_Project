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
        public string StatName;
        public Action<IEquipment, EquipmentStst, float> Apply;
        public Action<IEquipment, EquipmentStst, float> Remove;
    }

    private Dictionary<int, OptionMeta> optionTable = new();
    private Dictionary<int, string> DescriptionDic = new();

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
            StatName = "최대 생명력",
            Apply = (item, s, v) => { s.Hp += v; item.DescriptionDic[0] = $"최대 생명력 +{v} 증가"; },
            Remove = (item, s, v) => { s.Hp -= v; item.DescriptionDic.Remove(0); }
        };

        optionTable[1] = new OptionMeta
        {
            StatName = "최대 생명력%",
            Apply = (item, s, v) => { s.IncreaseHp += v / 100f; item.DescriptionDic[1] = $"최대 생명력 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseHp -= v / 100f; item.DescriptionDic.Remove(1); }
        };

        optionTable[2] = new OptionMeta
        {
            StatName = "생명력 재생 속도",
            Apply = (item, s, v) => { s.HpRegenRate += v; item.DescriptionDic[2] = $"생명력 재생 속도 +{v} 증가"; },
            Remove = (item, s, v) => { s.HpRegenRate -= v; item.DescriptionDic.Remove(2); }
        };

        optionTable[3] = new OptionMeta
        {
            StatName = "생명력 재생 속도%",
            Apply = (item, s, v) => { s.IncreaseHpRegenRate += v / 100f; item.DescriptionDic[3] = $"생명력 재생 속도 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseHpRegenRate -= v / 100f; item.DescriptionDic.Remove(3); }
        };

        optionTable[4] = new OptionMeta
        {
            StatName = "최대 마나",
            Apply = (item, s, v) => { s.Mp += v; item.DescriptionDic[4] = $"최대 마나 +{v} 증가"; },
            Remove = (item, s, v) => { s.Mp -= v; item.DescriptionDic.Remove(4); }
        };

        optionTable[5] = new OptionMeta
        {
            StatName = "최대 마나%",
            Apply = (item, s, v) => { s.IncreaseMp += v / 100f; item.DescriptionDic[5] = $"최대 마나 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseMp -= v / 100f; item.DescriptionDic.Remove(5); }
        };

        optionTable[6] = new OptionMeta
        {
            StatName = "마나 재생 속도",
            Apply = (item, s, v) => { s.MpRegenRate += v; item.DescriptionDic[6] = $"마나 재생 속도 +{v} 증가"; },
            Remove = (item, s, v) => { s.MpRegenRate -= v; item.DescriptionDic.Remove(6); }
        };

        optionTable[7] = new OptionMeta
        {
            StatName = "마나 재생 속도%",
            Apply = (item, s, v) => { s.IncreaseMpRegenRate += v / 100f; item.DescriptionDic[7] = $"마나 재생 속도 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseMpRegenRate -= v / 100f; item.DescriptionDic.Remove(7); }
        };

        optionTable[8] = new OptionMeta
        {
            StatName = "방어력",
            Apply = (item, s, v) => { s.Armour += v; item.DescriptionDic[8] = $"방어력 +{v} 증가"; },
            Remove = (item, s, v) => { s.Armour -= v; item.DescriptionDic.Remove(8); }
        };

        optionTable[9] = new OptionMeta
        {
            StatName = "방어력%",
            Apply = (item, s, v) => { s.IncreaseArmour += v / 100f; item.DescriptionDic[9] = $"방어력 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseArmour -= v / 100f; item.DescriptionDic.Remove(9); }
        };

        optionTable[10] = new OptionMeta
        {
            StatName = "회피",
            Apply = (item, s, v) => { s.Evasion += v; item.DescriptionDic[10] = $"회피 +{v} 증가"; },
            Remove = (item, s, v) => { s.Evasion -= v; item.DescriptionDic.Remove(10); }
        };

        optionTable[11] = new OptionMeta
        {
            StatName = "회피%",
            Apply = (item, s, v) => { s.IncreaseEvasion += v / 100f; item.DescriptionDic[11] = $"회피 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseEvasion -= v / 100f; item.DescriptionDic.Remove(11); }
        };

        optionTable[12] = new OptionMeta
        {
            StatName = "힘",
            Apply = (item, s, v) => { s.Strength += (int)v; item.DescriptionDic[12] = $"힘 +{(int)v}"; },
            Remove = (item, s, v) => { s.Strength -= (int)v; item.DescriptionDic.Remove(12); }
        };

        optionTable[13] = new OptionMeta
        {
            StatName = "민첩",
            Apply = (item, s, v) => { s.Dexterity += (int)v; item.DescriptionDic[13] = $"민첩 +{(int)v}"; },
            Remove = (item, s, v) => { s.Dexterity -= (int)v; item.DescriptionDic.Remove(13); }
        };

        optionTable[14] = new OptionMeta
        {
            StatName = "지능",
            Apply = (item, s, v) => { s.Intelligence += (int)v; item.DescriptionDic[14] = $"지능 +{(int)v}"; },
            Remove = (item, s, v) => { s.Intelligence -= (int)v; item.DescriptionDic.Remove(14); }
        };

        optionTable[15] = new OptionMeta
        {
            StatName = "이동속도%",
            Apply = (item, s, v) => { s.IncreaseMovementSpeed += v / 100f; item.DescriptionDic[15] = $"이동속도 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseMovementSpeed -= v / 100f; item.DescriptionDic.Remove(15); }
        };

        optionTable[16] = new OptionMeta
        {
            StatName = "공격속도%",
            Apply = (item, s, v) => { s.IncreaseAttackSpeed += v / 100f; item.DescriptionDic[16] = $"공격속도 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseAttackSpeed -= v / 100f; item.DescriptionDic.Remove(16); }
        };

        optionTable[17] = new OptionMeta
        {
            StatName = "시전속도%",
            Apply = (item, s, v) => { s.IncreaseCastSpeed += v / 100f; item.DescriptionDic[17] = $"시전속도 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseCastSpeed -= v / 100f; item.DescriptionDic.Remove(17); }
        };

        optionTable[18] = new OptionMeta
        {
            StatName = "치명타 확률%",
            Apply = (item, s, v) => { s.CriticalChance += v / 100f; item.DescriptionDic[18] = $"치명타 확률 +{v}% 증가"; },
            Remove = (item, s, v) => { s.CriticalChance -= v / 100f; item.DescriptionDic.Remove(18); }
        };

        optionTable[19] = new OptionMeta
        {
            StatName = "치명타 피해%",
            Apply = (item, s, v) => { s.CriticalDamage += v / 100f; item.DescriptionDic[19] = $"치명타 피해 +{v}% 증가"; },
            Remove = (item, s, v) => { s.CriticalDamage -= v / 100f; item.DescriptionDic.Remove(19); }
        };

        optionTable[20] = new OptionMeta
        {
            StatName = "공격력 최소값",
            Apply = (item, s, v) => { s.MinAttackDamage += (int)v; item.DescriptionDic[20] = $"공격력 +{(int)v}"; },
            Remove = (item, s, v) => { s.MinAttackDamage -= (int)v; item.DescriptionDic.Remove(20); }
        };

        optionTable[21] = new OptionMeta
        {
            StatName = "공격력%",
            Apply = (item, s, v) => { s.IncreaseAttackDamage += v / 100f; item.DescriptionDic[21] = $"공격력 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseAttackDamage -= v / 100f; item.DescriptionDic.Remove(21); }
        };

        optionTable[22] = new OptionMeta
        {
            StatName = "주문력 최소값",
            Apply = (item, s, v) => { s.MinSpellDamage += (int)v; item.DescriptionDic[22] = $"주문력 +{(int)v}"; },
            Remove = (item, s, v) => { s.MinSpellDamage -= (int)v; item.DescriptionDic.Remove(22); }
        };

        optionTable[23] = new OptionMeta
        {
            StatName = "주문력%",
            Apply = (item, s, v) => { s.IncreaseSpellDamage += v / 100f; item.DescriptionDic[23] = $"주문력 +{v}% 증가"; },
            Remove = (item, s, v) => { s.IncreaseSpellDamage -= v / 100f; item.DescriptionDic.Remove(23); }
        };
    }

    public void EquipEquipment(Item item)
    {
        if (item is not IEquipment equip) return;

        string str = $"장비명: {item.ItemData.Name}\n" +
                     $"방어력: {equip.Armor} | 회피: {equip.Evasion}\n" +
                     $"등급: {equip.Rarity}\n" +
                     "================\n";
        
        foreach (var kv in equip.CashingValue)
        {
            if (optionTable.TryGetValue(kv.Key, out var meta))
            {
                meta.Apply(equip, EquipmentStat, kv.Value);
                str += $"{equip.DescriptionDic[kv.Key]}\n";
                //액션에서 추가하는 디스크립션이랑 여기랑 달라서 null이 뜸
            }
        }

        Debug.Log(str);
        Player.LocalPlayer.RealStat.UpdateStat();
    }

    public void UnEquipEquipment(Item item)
    {
        if (item is not IEquipment equip) return;

        foreach (var kv in equip.CashingValue)
        {
            if (optionTable.TryGetValue(kv.Key, out var meta))
            {
                meta.Remove(equip, EquipmentStat, kv.Value);
            }
        }

        Player.LocalPlayer.RealStat.UpdateStat();
    }
}
