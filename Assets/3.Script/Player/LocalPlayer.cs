using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LocalPlayer  : Player, IDamageAble
{
    public Collider MainCollider => playerCollider;
    public GameObject GameObject =>  gameObject;

    public Weapon CurrentWeapon;
    public PlayerStat Stat { get; private set; }

    [SerializeField] private Collider playerCollider;
    
    private void Awake()
    {
        Player.LocalPlayer = this;
        
        Stat = new PlayerStat();
        Stat.MaxHP = 100;
        Stat.HP = Stat.MaxHP;
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}
