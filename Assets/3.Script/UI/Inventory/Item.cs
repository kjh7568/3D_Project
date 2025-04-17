using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Sprite Sprite;
    public ItemData ItemData { get; set; }
}

public class ItemData
{
    public string Name { get; set; }
    public int Key { get; set; }
    public string ItemType { get; set; }
    
    /// <summary>
    /// 특수한 문자열로 아이템의 기능 매개변수를 넣는다 --> 쓸 일이 있을까
    /// </summary>
    public string Parameter { get; set; }
}