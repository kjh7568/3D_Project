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
        if (!other.CompareTag("Player")) return;
        
        MonsterStat monsterStat = Owner.GetStat();
        
        CombatEvent combatEvent = new CombatEvent
        {
            Sender = Owner,
            Receiver = Player.LocalPlayer,
            HitPosition = other.ClosestPoint(transform.position),
            Collider = other
        };

        if (Player.LocalPlayer.RealStat.EvasionRate > Random.Range(0f, 1f))
        {
            combatEvent.Damage = 0;
        }
        else
        {
            combatEvent.Damage = Random.Range(monsterStat.minDamage, monsterStat.maxDamage) * (1 - Player.LocalPlayer.RealStat.DamageReductionRate);
        }
        
        CombatSystem.Instance.AddInGameEvent(combatEvent);
    }
}
