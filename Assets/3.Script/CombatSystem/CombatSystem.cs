using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance;

    private const int MAX_EVENT_PROCESS_COUNT = 10;

    public class Callbacks
    {
        //CombatEvent가 발생하면의 의미로 쓸거임
        public Action<CombatEvent> OnCombatEvent;
    }

    public readonly Callbacks Events = new Callbacks();

    private Dictionary<Collider, IDamageAble> monsterDic = new Dictionary<Collider, IDamageAble>();
    private Queue<InGameEvent> inGameEventQueue = new Queue<InGameEvent>();
    
    private void Awake()
    {
        Instance = this;
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
}
