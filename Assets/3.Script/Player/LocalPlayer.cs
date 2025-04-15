using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer  : Player, IDamageAble
{
    public Collider MainCollider => collider;
    public GameObject GameObject =>  gameObject;
    
    public Weapon CurrentWeapon { get; set; }
    public PlayerStat Stat { get; private set; }

    [SerializeField] private Collider collider;
    
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
