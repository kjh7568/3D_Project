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

    // 특수한 문자열로 아이템의 기능 매개변수를 넣는다 --> 쓸 일이 있을까 --> 겁나 필요하네ㅋㅋ --> 파라미터는 신이야!
    public string Parameter { get; set; }

    // 아이템요구 능력치
    public int RequiredStrength { get; set; }
    public int RequiredDexterity { get; set; }
    public int RequiredIntelligence { get; set; }
    public int RequiredLevel { get; set; }
}