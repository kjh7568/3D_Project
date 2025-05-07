using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobilnWarChief : MonoBehaviour, IMonster
{
    public Collider MainCollider => monsterCollider;
    public GameObject GameObject => gameObject;
    public Collider AttackCollider => attackCollider;
    public MonsterStat MonsterStat => monsterStat;

    [SerializeField] private MonsterStat monsterStat;
    [SerializeField] private Collider monsterCollider;
    [SerializeField] private Collider attackCollider;
    
    [SerializeField] private int dropGoldMinAmount = 10;
    [SerializeField] private int dropGoldMaxAmount = 18;

    private MonsterController monster;

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

    private void OnDead()
    {
        monsterCollider.enabled = false;
        AttackCollider.enabled = false;

        Player.LocalPlayer.Stat.Exp += monsterStat.rewardExp;

        TryGenerateItem();

        monster.PlayDead();
    }
    
    public MonsterStat GetStat() => monsterStat;
    
    public void TryGenerateItem()
    {
        if (Random.Range(0f, 1f) < 0.40f) // 40% 확률
        {
            RewardManager.Instance.DropItem(transform.position);
        }

        if (Random.Range(0f, 1f) < 0.7f) // 70% 확률
        {
            RewardManager.Instance.DropGold(dropGoldMinAmount, dropGoldMaxAmount, transform.position);
        }
    }
}
