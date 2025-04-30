using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [System.Serializable]
    public class SkillData
    {
        public string skillName;
        public float damageRate;
        public float moveSpeed;
        public float costMana;
    }

    public SkillData data;
    public List<string> tags = new List<string>();
    public abstract void Cast();
    
    public float CalculateDamage()
    {
        var pStat = Player.LocalPlayer.RealStat;
        var damage = (Random.Range(pStat.MinSpellDamage, pStat.MaxSpellDamage) * pStat.IncreaseSpellDamage) * data.damageRate;
        
        if (Random.Range(0f, 1f) < pStat.CriticalChance)
        {
            Debug.Log($"Critical Hit!: {damage + (damage * pStat.CriticalDamage)}");
            return damage + (damage * pStat.CriticalDamage);
        }
        else
        {
            Debug.Log($"Normal Hit: {damage}");
            return damage;
        }
    }
}
