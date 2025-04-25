using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon : IEquipment
{
    float MinAttackDamage { get; set; }
    float MaxAttackDamage { get; set; }
    float MinSpellDamage { get; set; }
    float MaxSpellDamage { get; set; }
}

public class OneHandSword : Item, IWeapon
{
    public int Rarity { get; set; }
    public List<int> OptionIdx { get; set; } = new List<int>();
    public Dictionary<int, int> CashingValue { get; set; } = new Dictionary<int, int>();
    public Dictionary<int, string> DescriptionDic { get; set; } = new Dictionary<int, string>();

    public float MinAttackDamage { get; set; }
    public float MaxAttackDamage { get; set; }
    public float MinSpellDamage { get; set; }
    public float MaxSpellDamage { get; set; }

    public int PickRandomOption()
    {
        float random = Random.Range(0f, 100f);

        if (random < 7f) return 16; // 공격속도 +#%
        if (random < 14f) return 18; // 치명타 확률 +#%
        if (random < 19f) return 19; // 치명타 피해 +#%
        if (random < 24f) return 21; // 공격력 +#%

        float remaining = random - 24f;
        float perOption = 76f / 5f; // ≒ 15.2%

        if (remaining < perOption * 1) return 0; // 최대 생명력
        if (remaining < perOption * 2) return 3; // 생명력 재생 속도 +#%
        if (remaining < perOption * 3) return 12; // 힘
        if (remaining < perOption * 4) return 13; // 민첩
        return 20; // 공격력 추가 +# ~ +#
    }
}

public class Wand : Item, IWeapon
{
    public int Rarity { get; set; }
    public List<int> OptionIdx { get; set; } = new List<int>();
    public Dictionary<int, int> CashingValue { get; set; } = new Dictionary<int, int>();
    public Dictionary<int, string> DescriptionDic { get; set; } = new Dictionary<int, string>();

    public float MinAttackDamage { get; set; }
    public float MaxAttackDamage { get; set; }
    public float MinSpellDamage { get; set; }
    public float MaxSpellDamage { get; set; }

    public int PickRandomOption()
    {
        float random = Random.Range(0f, 100f);

        if (random < 7f) return 17; // 시전속도 +#%
        if (random < 14f) return 18; // 치명타 확률 +#%
        if (random < 19f) return 19; // 치명타 피해 +#%
        if (random < 24f) return 23; // 주문력 +#%

        float remaining = random - 24f;
        float perOption = 76f / 4f; // 19%

        if (remaining < perOption * 1) return 4; // 최대 마나 +#
        if (remaining < perOption * 2) return 7; // 마나 재생 속도 +#%
        if (remaining < perOption * 3) return 14; // 지능
        return 22; // 주문력 +# ~ +#
    }
}