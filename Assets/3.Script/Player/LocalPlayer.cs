using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LocalPlayer  : Player, IDamageAble
{
    public Collider MainCollider => playerCollider;
    public GameObject GameObject =>  gameObject;

    public Weapon currentWeapon;
    public PlayerStat Stat { get; private set; }

    [SerializeField] private Collider playerCollider;
    
    private void Awake()
    {
        Player.LocalPlayer = this;
        
        Stat = new PlayerStat();
        Stat.Initialize();
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        Stat.Hp -= combatEvent.Damage;
        Debug.Log($"{combatEvent.Damage}의 데미지를 받음");
    }
}
