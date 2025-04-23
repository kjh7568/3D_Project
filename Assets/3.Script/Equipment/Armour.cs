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
        float random = Random.Range(5f, 15f) /*Random.Range(0f, 100f)*/;

        if (random < 2.5f) return 1;
        if (random < 5.0f) return 5;
        if (random < 15.0f) return 0;
        if (random < 25.0f) return 4;
        if (random < 35.0f) return 8;
        if (random < 45.0f) return 10;

        float remaining = random - 45.0f;
        float perOption = 55.0f / 7.0f;

        if (remaining < perOption * 1) return 3;
        if (remaining < perOption * 2) return 7;
        if (remaining < perOption * 3) return 9;
        if (remaining < perOption * 4) return 11;
        if (remaining < perOption * 5) return 12;
        if (remaining < perOption * 6) return 13;
        return 14;
    }
}

public class Gloves : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; }
    public Dictionary<int, int> CashingValue { get; set; }
    public List<string> DescriptionList { get; set; }
    public int PickRandomOption()
    {
        throw new System.NotImplementedException();
    }
}

public class Helmet : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; }
    public Dictionary<int, int> CashingValue { get; set; }
    public List<string> DescriptionList { get; set; }
    public int PickRandomOption()
    {
        throw new System.NotImplementedException();
    }
}

public class Boots : Item, IEquipment
{
    public int Rarity { get; set; }
    public float Armor { get; set; }
    public float Evasion { get; set; }
    public List<int> OptionIdx { get; set; }
    public Dictionary<int, int> CashingValue { get; set; }
    public List<string> DescriptionList { get; set; }
    public int PickRandomOption()
    {
        throw new System.NotImplementedException();
    }
}