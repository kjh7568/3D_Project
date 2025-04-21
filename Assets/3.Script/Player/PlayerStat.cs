public class PlayerStat
{
    public float Hp { get; set; }
    public float MaxHp { get; set; }
        
    public float Mp { get; set; }
    public float MaxMp { get; set; }
        
    public int Level { get; set; }
    public float Exp { get; set; }
    public float MaxExp { get; set; }
        
    public float Armor { get; set; }
    public float Evasion { get; set; }
        
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }

    public void Initialize()
    {
        MaxHp = 100;
        Hp = MaxHp;
        
        MaxMp = 30;
        Mp = MaxMp;

        Level = 1;
        Exp = 0;
        MaxExp = 10;

        Strength = 5;
        Dexterity = 5;
        Intelligence = 5;
            
        Armor = 0;
        Evasion = 0;
    }

    public void UpdateMaxExp()
    {
        MaxExp += MaxExp * 1.2f;
    }

    public void AddingStatFromAttributes()
    {
        // 이거 이래도 되나?
    }
}