using System;
using UnityEngine;

public class EquipmentStst
{
    // 연산 순서는 플랫 더하고 increase들 곱한다.
    public float Hp { get; set; }
    public float IncreaseHp { get; set; }
    public float HpRegenRate { get; set; }
    public float IncreaseHpRegenRate { get; set; }

    public float Mp { get; set; }
    public float IncreaseMp { get; set; }
    public float MpRegenRate { get; set; }
    public float IncreaseMpRegenRate { get; set; }

    public float Armour { get; set; }
    public float IncreaseArmour { get; set; }
    public float Evasion { get; set; }
    public float IncreaseEvasion { get; set; }

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }

    public float IncreaseMovementSpeed { get; set; }
    public float IncreaseAttackSpeed { get; set; }
    public float IncreaseCastSpeed { get; set; }

    public float CriticalChance { get; set; }
    public float CriticalDamage { get; set; }

    public int MinAttackDamage { get; set; }
    public int MaxAttackDamage { get; set; }
    public float IncreaseAttackDamage { get; set; }

    public int MinSpellDamage { get; set; }
    public int MaxSpellDamage { get; set; }
    public float IncreaseSpellDamage { get; set; }

    public void Initialize()
    {
        Hp = 0f;
        IncreaseHp = 1f;
        HpRegenRate = 0f;
        IncreaseHpRegenRate = 1f;

        Mp = 0f;
        IncreaseMp = 1f;
        MpRegenRate = 0f;
        IncreaseMpRegenRate = 1f;

        Armour = 0f;
        IncreaseArmour = 1f;
        Evasion = 0f;
        IncreaseEvasion = 1f;

        Strength = 0;
        Dexterity = 0;
        Intelligence = 0;

        IncreaseMovementSpeed = 1f;
        IncreaseAttackSpeed = 1f;
        IncreaseCastSpeed = 1f;

        CriticalChance = 0f;
        CriticalDamage = 0f;

        MinAttackDamage = 0;
        MaxAttackDamage = 0;
        IncreaseAttackDamage = 1f;

        MinSpellDamage = 0;
        MaxSpellDamage = 0;
        IncreaseSpellDamage = 1f;
    }
}