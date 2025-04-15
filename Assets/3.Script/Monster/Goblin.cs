using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour, IDamageAble
{
    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;
    
    [System.Serializable]
    public class MonsterStat
    {
        public string name;
        public int hp;
        public int maxHp;
        public float range;
        public float speed;
    }

    [SerializeField] private MonsterStat monsterStat;
    [SerializeField] private Collider collider;
    
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

    private void OnDead()
    {
        //todo 사망 판정 만들기
        collider.enabled = false;
        monster.PlayDead();
    }
}
