using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    /// <summary>
    /// 무기 이름
    /// </summary>
    public string WeaponName;

    /// <summary>
    /// 최소 데미지
    /// </summary>
    public int MinDamage;

    /// <summary>
    /// 최대 데미지
    /// </summary>
    public int MaxDamage;

    /// <summary>
    /// 공격 속도
    /// </summary>
    public float AttackSpeed;
}