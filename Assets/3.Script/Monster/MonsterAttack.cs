using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private IMonster Owner { get; set; }

    private void Start()
    {
        Owner = gameObject.GetComponentInParent<IMonster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        
        MonsterStat monsterStat = Owner.GetStat();
        
        CombatEvent combatEvent = new CombatEvent
        {
            Sender = Owner,
            Receiver = Player.LocalPlayer,
            Damage = Random.Range(monsterStat.minDamage, monsterStat.maxDamage),
            HitPosition = other.ClosestPoint(transform.position),
            Collider = other
        };
        
        CombatSystem.Instance.AddInGameEvent(combatEvent);
    }
}
