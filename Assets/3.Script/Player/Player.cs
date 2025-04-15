using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static LocalPlayer LocalPlayer;

    public class PlayerStat
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        
        //todo 패시브 만들면 여기에 스탯 추가
    }
}
