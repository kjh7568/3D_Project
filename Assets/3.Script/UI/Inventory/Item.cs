using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Sprite Sprite;
    public Vector2 DragSize { get; set; }
    
    public ItemData ItemData { get; set; }
}

public class ItemData
{
    public string Name { get; set; }
    public int Key { get; set; }
    public string ItemType { get; set; }

    public string Parameter { get; set; }

    // 아이템요구 능력치
    public int RequiredStrength { get; set; }
    public int RequiredDexterity { get; set; }
    public int RequiredIntelligence { get; set; }
    public int RequiredLevel { get; set; }
}