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

    
    // Start is called before the first frame update
    void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        monsterStat.hp -= combatEvent.Damage;

        Debug.Log($"현재 {monsterStat.name} 체력: {monsterStat.hp}/{monsterStat.maxHp}");
        
        if (monsterStat.hp <= 0)
        {
            //todo 사망 판정 만들기
        }
    }
}
