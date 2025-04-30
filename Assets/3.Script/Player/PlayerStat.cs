using UnityEngine;

public class PlayerStat
{
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
    public float Evasion { get; set; }

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    
    public float MovementSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public float CastSpeed { get; set; }

    public void Initialize()
    {
        MaxHp = 50;
        Hp = MaxHp;
        HpRegenRate = 1;

        MaxMp = 30;
        Mp = MaxMp;
        MpRegenRate = 1;

        Level = 1;
        Exp = 0;
        MaxExp = 10;

        Strength = 5;
        Dexterity = 12;
        Intelligence = 5;

        Armour = 0;
        Evasion = 0;

        MovementSpeed = 7;
        AttackSpeed = 1;
        CastSpeed = 1;
    }


    public void CheckLevelUp()
    {
        if (Exp >= MaxExp)
        {
            Level++;
            UpdateMaxExp();
            GetLevelUpReward();
        }
    }

    private void UpdateMaxExp()
    {
        MaxExp += Mathf.CeilToInt(MaxExp * 1.33f);
        Exp = 0;
    }

    private void GetLevelUpReward()
    {
        Strength += 1;
        Dexterity += 1;
        Intelligence += 1;
    }
}