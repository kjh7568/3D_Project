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

    //todo 나중에 지팡이도 추가
    public Collider weaponCollider;
    public PlayerStat Stat { get; private set; }
    public int gold;
    public FinalPlayerStats RealStat { get; private set; }

    [SerializeField] private Collider playerCollider;

    private void Start()
    {
        Player.LocalPlayer = this;
        
        if (GameDataSync.Instance.playerStat != null)
        {
            Stat = GameDataSync.Instance.playerStat;
            gold = GameDataSync.Instance.gold;
        }
        else
        {
            Stat = new PlayerStat();
            Stat.Initialize();
        }

        RealStat = new FinalPlayerStats();
        RealStat.UpdateStat();
        RealStat.Initialize();
    }

    private void Update()
    {
        RegenerateResources();
        Stat.CheckLevelUp();
    }

    private void OnDestroy()
    {
        GameDataSync.Instance.playerStat = Stat;
        GameDataSync.Instance.gold = gold;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        RealStat.Hp -= combatEvent.Damage;
    }

    private void RegenerateResources()
    {
        if (RealStat.Hp < RealStat.MaxHp)
        {
            RealStat.Hp += RealStat.HpRegenRate * Time.deltaTime;
        }

        if (RealStat.Mp < RealStat.MaxMp)
        {
            RealStat.Mp += RealStat.MpRegenRate * Time.deltaTime;
        }
    }
}