using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public interface IEquipment
{
    int Rarity { get; set; }
    float Armor { get; set; }
    float Evasion { get; set; }
    List<int> OptionIdx { get; set; }
    Dictionary<int, int> CashingValue { get; set; }
    List<string> DescriptionList { get; set; }

    int PickRandomOption();
}

public class BodyArmour : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; } = new List<int>();
    public Dictionary<int, int> CashingValue { get; set; } = new Dictionary<int, int>(); // 인덱스, 데이터
    public List<string> DescriptionList { get; set; } = new List<string>();

    public int PickRandomOption()
    {
        // float random = Random.Range(0f, 100f);
        //
        // if (random < 2.5f) return 1;
        // if (random < 5.0f) return 5;
        // if (random < 15.0f) return 0;
        // if (random < 25.0f) return 4;
        // if (random < 35.0f) return 8;
        // if (random < 45.0f) return 10;
        //
        // float remaining = random - 45.0f;
        // float perOption = 55.0f / 7.0f;
        //
        // if (remaining < perOption * 1) return 3;
        // if (remaining < perOption * 2) return 7;
        // if (remaining < perOption * 3) return 9;
        // if (remaining < perOption * 4) return 11;
        // if (remaining < perOption * 5) return 12;
        // if (remaining < perOption * 6) return 13;
        // return 14;

        return 0;
    }
}

public class Gloves : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; } = new List<int>();
    public Dictionary<int, int> CashingValue { get; set; } = new Dictionary<int, int>();
    public List<string> DescriptionList { get; set; } = new List<string>();

    public int PickRandomOption()
    {
        // float random = Random.Range(0f, 100f);
        //
        // // 고정 확률 옵션 처리
        // if (random < 1f) return 1;
        // if (random < 2f) return 5;
        // if (random < 3f) return 18;
        //
        // if (random < 5.5f) return 16;
        // if (random < 8.0f) return 20;
        // if (random < 10.5f) return 22;
        //
        // // 균등 확률 분배 옵션 (9개, 89.5% / 9 = 약 9.944%)
        // float remaining = random - 10.5f;
        // float perOption = (100f - 10.5f) / 9f;
        //
        // if (remaining < perOption * 1) return 0;
        // if (remaining < perOption * 2) return 4;
        // if (remaining < perOption * 3) return 8;
        // if (remaining < perOption * 4) return 9;
        // if (remaining < perOption * 5) return 10;
        // if (remaining < perOption * 6) return 11;
        // if (remaining < perOption * 7) return 12;
        // if (remaining < perOption * 8) return 13;
        // return 14;

        return 0;
    }
}

public class Helmet : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; } = new List<int>();
    public Dictionary<int, int> CashingValue { get; set; } = new Dictionary<int, int>();
    public List<string> DescriptionList { get; set; } = new List<string>();

    public int PickRandomOption()
    {
        // float random = Random.Range(0f, 100f);
        //
        // // 고정 확률 우선 분기
        // if (random < 3f) return 16;
        // if (random < 6f) return 17;
        // if (random < 21f) return 8;
        // if (random < 36f) return 10;
        // if (random < 43.5f) return 9;
        // if (random < 51f) return 11;
        //
        // // 균등 분배 대상: 0, 2, 3, 4, 6, 7, 12, 13, 14
        // float remaining = random - 51f;
        // float perOption = (100f - 51f) / 9f; // ≒ 5.444%
        //
        // if (remaining < perOption * 1) return 0;
        // if (remaining < perOption * 2) return 2;
        // if (remaining < perOption * 3) return 3;
        // if (remaining < perOption * 4) return 4;
        // if (remaining < perOption * 5) return 6;
        // if (remaining < perOption * 6) return 7;
        // if (remaining < perOption * 7) return 12;
        // if (remaining < perOption * 8) return 13;
        // return 14;

        return 0;
    }
}

public class Boots : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; } = new List<int>();
    public Dictionary<int, int> CashingValue { get; set; } = new Dictionary<int, int>();
    public List<string> DescriptionList { get; set; } = new List<string>();

    public int PickRandomOption()
    {
        // float random = Random.Range(0f, 100f);
        //
        // if (random < 15f) return 15;
        //
        // // 균등 분배 대상: 0, 2, 4, 6, 8, 9, 10, 11, 13, 14, 18, 22
        // float remaining = random - 15f;
        // float perOption = (100f - 15f) / 12f; // ≒ 7.08%
        //
        // if (remaining < perOption * 1) return 0;
        // if (remaining < perOption * 2) return 2;
        // if (remaining < perOption * 3) return 4;
        // if (remaining < perOption * 4) return 6;
        // if (remaining < perOption * 5) return 8;
        // if (remaining < perOption * 6) return 9;
        // if (remaining < perOption * 7) return 10;
        // if (remaining < perOption * 8) return 11;
        // if (remaining < perOption * 9) return 13;
        // if (remaining < perOption * 10) return 14;
        // return 18;

        return 0;
    }
}