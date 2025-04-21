using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance;

    private const int MAX_EVENT_PROCESS_COUNT = 10;
    private const int MAX_BLOOD_COUNT = 30;

    [SerializeField] private Transform bloodPrefabParent;
    
    private WaitForSeconds bloodWait = new WaitForSeconds(2f);
    public class Callbacks
    {
        //CombatEvent가 발생하면의 의미로 쓸거임
        public Action<CombatEvent> OnCombatEvent;
    }

    public readonly Callbacks Events = new Callbacks();

    private Dictionary<Collider, IDamageAble> monsterDic = new Dictionary<Collider, IDamageAble>();
    private Queue<InGameEvent> inGameEventQueue = new Queue<InGameEvent>();
    private Queue<GameObject> bloodPrefabQueue = new Queue<GameObject>();
    [SerializeField] private GameObject bloodPrefab;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < MAX_BLOOD_COUNT; i++)
        {
            var blood = Instantiate(bloodPrefab, bloodPrefabParent);
            bloodPrefabQueue.Enqueue(blood);
            blood.SetActive(false);
        }
    }

    private void Update()
    {
        int processCount = 0;
        while (inGameEventQueue.Count > 0 && processCount < MAX_EVENT_PROCESS_COUNT)
        {
            var inGameEvent = inGameEventQueue.Dequeue();

            switch (inGameEvent.Type)
            {
                case InGameEvent.EventType.Combat:
                    var combatEvent = inGameEvent as CombatEvent;
                    inGameEvent.Receiver.TakeDamage(combatEvent);
                    StartCoroutine(SpawnBloodEffect(combatEvent.HitPosition));
                    Events.OnCombatEvent?.Invoke(combatEvent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void RegisterMonster(IDamageAble monster)
    {
        if (monsterDic.TryAdd(monster.MainCollider, monster) == false)
        {
            Debug.LogWarning($"{monster.GameObject.name}가 등록되어 있습니다." +
                             $"{monsterDic[monster.MainCollider]}를 대체합니다");
            monsterDic[monster.MainCollider] = monster;
        }
    }

    public IDamageAble GetMonsterOrNull(Collider collider)
    {
        if (monsterDic.ContainsKey(collider))
        {
            return monsterDic[collider];
        }

        return null;
    }

    public void AddInGameEvent(InGameEvent inGameEvent)
    {
        inGameEventQueue.Enqueue(inGameEvent);
    }

    private IEnumerator SpawnBloodEffect(Vector3 hitPosition)
    {
        var blood = bloodPrefabQueue.Dequeue();
        blood.transform.position = hitPosition;
        blood.SetActive(true);
        
        yield return bloodWait;
        
        blood.SetActive(false);
        bloodPrefabQueue.Enqueue(blood);
    }
}