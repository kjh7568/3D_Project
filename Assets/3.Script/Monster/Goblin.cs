using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Goblin : MonoBehaviour, IMonster
{
    public Collider MainCollider => monsterCollider;
    public GameObject GameObject => gameObject;
    public Collider AttackCollider => attackCollider;

    [SerializeField] private MonsterStat monsterStat;
    [SerializeField] private Collider monsterCollider;
    [SerializeField] private Collider attackCollider;
    
    private MonsterController monster;
    
    // Start is called before the first frame update
    void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
        monster = GetComponent<MonsterController>();
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        monsterStat.hp -= combatEvent.Damage;

        Debug.Log($"현재 {monsterStat.name} 체력: {monsterStat.hp}/{monsterStat.maxHp}");
        
        if (monsterStat.hp <= 0)
        {
            OnDead();
        }
    }

    public MonsterStat GetStat() => monsterStat;

    private void OnDead()
    {
        monsterCollider.enabled = false;
        AttackCollider.enabled = false;
        
        monster.PlayDead();
    }
}
