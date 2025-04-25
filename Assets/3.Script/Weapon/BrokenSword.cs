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
                Damage = Random.Range(Player.LocalPlayer.RealStat.MinAttackDamage, Player.LocalPlayer.RealStat.MaxAttackDamage) * Player.LocalPlayer.RealStat.IncreaseAttackDamage,
                HitPosition = other.ClosestPoint(transform.position),
                Collider = other
            };

            CombatSystem.Instance.AddInGameEvent(combatEvent);
        }
    }
}
