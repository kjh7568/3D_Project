using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayerStats
{
    // 연산 순서는 (플레이어 + 장비) * 패시브    
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public float HpRegenRate { get; set; }

    public float Mp { get; set; }
    public float MaxMp { get; set; }
    public float MpRegenRate { get; set; }

    public int Level { get; set; }
    public float Exp { get; set; }
    public float MaxExp { get; set; }

    public float Armour { get; set; }
    public float DamageReductionRate { get; set; }
    public float Evasion { get; set; }
    public float EvasionRate { get; set; }
    
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }

    // 추가로 필요한 멤버 변수
    public float MovementSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public float CastSpeed { get; set; }

    public float CriticalChance { get; set; }
    public float CriticalDamage { get; set; }

    public float MinAttackDamage { get; set; }
    public float MaxAttackDamage { get; set; }
    public float IncreaseAttackDamage { get; set; }

    public float MinSpellDamage { get; set; }
    public float MaxSpellDamage { get; set; }
    public float IncreaseSpellDamage { get; set; }

    public void Initialize()
    {
        Hp = MaxHp;
        Mp = MaxMp;
    }

    public void UpdateStat()
    {
        PlayerStat pStat = Player.LocalPlayer.Stat;
        EquipmentStst eStat = EquipmentManager.Instance.EquipmentStat;

        // 체력
        MaxHp = Mathf.Round((pStat.Hp + eStat.Hp) * eStat.IncreaseHp);
        if (Hp > MaxHp)
        {
            Hp = MaxHp;
        }
        HpRegenRate = (pStat.HpRegenRate + eStat.HpRegenRate) * eStat.IncreaseHpRegenRate;

        // 마나
        MaxMp = Mathf.Round((pStat.Mp + eStat.Mp) * eStat.IncreaseMp);
        if (Mp > MaxMp)
        {
            Mp = MaxMp;
        }
        MpRegenRate = (pStat.MpRegenRate + eStat.MpRegenRate) * eStat.IncreaseMpRegenRate;

        // 방어/회피
        Armour = (pStat.Armour + eStat.Armour) * eStat.IncreaseArmour;
        DamageReductionRate = CalculateDamageReduction(Armour);
        Evasion = (pStat.Evasion + eStat.Evasion) * eStat.IncreaseEvasion;
        EvasionRate = CalculateEvasionChance(Evasion);
        
        // 스탯
        Strength = pStat.Strength + eStat.Strength;
        Dexterity = pStat.Dexterity + eStat.Dexterity;
        Intelligence = pStat.Intelligence + eStat.Intelligence;

        // 이동속도, 공격속도, 시전속도
        MovementSpeed = pStat.MovementSpeed * eStat.IncreaseMovementSpeed;
        AttackSpeed = pStat.AttackSpeed * eStat.IncreaseAttackSpeed;
        CastSpeed = pStat.CastSpeed * eStat.IncreaseCastSpeed;

        // EquipmentStst만 있는 항목
        CriticalChance = eStat.CriticalChance;
        CriticalDamage = eStat.CriticalDamage;

        MinAttackDamage = eStat.MinAttackDamage;
        MaxAttackDamage = eStat.MaxAttackDamage;
        IncreaseAttackDamage = eStat.IncreaseAttackDamage;

        MinSpellDamage = eStat.MinSpellDamage;
        MaxSpellDamage = eStat.MaxSpellDamage;
        IncreaseSpellDamage = eStat.IncreaseSpellDamage;
    }
    
    private float CalculateDamageReduction(float armour)
    {
        const float k = 233.33f;
        return armour / (armour + k);
    }
    
    private float CalculateEvasionChance(float evasion)
    {
        const float K = 400f;
        return evasion / (evasion + K);
    }
}
