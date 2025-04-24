using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Goblin : MonoBehaviour, IMonster
{
    public Collider MainCollider => monsterCollider;
    public GameObject GameObject => gameObject;
    public Collider AttackCollider => attackCollider;
    public MonsterStat MonsterStat => monsterStat;

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

        Player.LocalPlayer.RealStat.Exp += monsterStat.rewardExp;

        TryGenerateItem();
        
        monster.PlayDead();
    }

    public void TryGenerateItem()
    {
        float chance = Random.Range(0f, 1f); // 0.0 ~ 1.0 사이

        if (chance < 0.33f) // 33% 확률
        {
            Debug.Log("아이템 드랍!");
            RewardManager.Instance.DropItem();
        }
    }
}
