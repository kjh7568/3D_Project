using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = UnityEngine.Random;

public class BrokenSword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        
        var monster = CombatSystem.Instance.GetMonsterOrNull(other);
        
        if (monster != null)
        {
            CombatEvent combatEvent = new CombatEvent
            {
                Sender = Player.LocalPlayer,
                Receiver = monster,
                Damage = CalculateDamage(),
                HitPosition = other.ClosestPoint(transform.position),
                Collider = other
            };

            CombatSystem.Instance.AddInGameEvent(combatEvent);
        }
    }
    
    private float CalculateDamage()
    {
        var pStat = Player.LocalPlayer.RealStat;

        if (Random.Range(0f, 1f) < pStat.CriticalChance)
        {
            Debug.Log("Critical Hit!");

            var temp = Random.Range(pStat.MinAttackDamage, pStat.MaxAttackDamage) * pStat.IncreaseAttackDamage;
            
            return temp + (temp * pStat.CriticalDamage);
        }
        else
        {
            return Random.Range(pStat.MinAttackDamage, pStat.MaxAttackDamage) * pStat.IncreaseAttackDamage;
        }
    }
}
