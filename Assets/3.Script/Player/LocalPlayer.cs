using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LocalPlayer : Player, IDamageAble
{
    public Collider MainCollider => playerCollider;
    public GameObject GameObject => gameObject;

    public Weapon currentWeapon;
    public PlayerStat Stat { get; private set; }
    public FinalPlayerStats RealStat { get; private set; }

    [SerializeField] private Collider playerCollider;

    private void Start()
    {
        Player.LocalPlayer = this;

        Stat = new PlayerStat();
        Stat.Initialize();
        
        RealStat= new FinalPlayerStats();
        RealStat.UpdateStat();
        RealStat.Initialize();
    }

    private void Update()
    {
        RegenerateResources();
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        RealStat.Hp -= combatEvent.Damage;
        Debug.Log($"{combatEvent.Damage}의 데미지를 받음");
    }

    private void RegenerateResources()
    {
        if (RealStat.Hp < RealStat.MaxHp)
        {
            RealStat.Hp += RealStat.HpRegenRate * Time.deltaTime;
        }

        if(RealStat.Mp < RealStat.MaxMp)
        {
            RealStat.Mp += RealStat.MpRegenRate * Time.deltaTime;
        }
    }
}