using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    private const int MAX_SKILL_COUNT = 3;
    private const int MAX_PREFAB_COUNT = 10;

    public List<GameObject> skillPrefabs = new List<GameObject>();
    public Queue<GameObject>[] skillPool = new Queue<GameObject>[MAX_SKILL_COUNT];
    public bool[] isInSkill = new bool[MAX_SKILL_COUNT];
    public int currentCastingSpellIndex;

    [SerializeField] private Transform[] poolParents = new Transform[MAX_SKILL_COUNT];
    [SerializeField] private Transform firePoint;

    private Action<Skill>[] addComponentHandler = new Action<Skill>[100];
    private Action<Skill>[] removeComponentHandler = new Action<Skill>[100];

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < MAX_SKILL_COUNT; i++)
        {
            skillPool[i] = new Queue<GameObject>();
        }
        
        if (GameDataSync.Instance != null)
        {
            isInSkill = GameDataSync.Instance.isInSkill;
        }
        else
        {
            for (int i = 0; i < MAX_SKILL_COUNT; i++)
            {
                isInSkill[i] = false;
            }
        }
    }

    private void Start()
    {
        InitializeHandler();
        InitializeSkillPoolOnSceneLoad();
    }

    private void OnDestroy()
    {
        GameDataSync.Instance.isInSkill = isInSkill;
    }
    
    public void InitializeHandler()
    {
        addComponentHandler[0] = (skill) => { skill.Add<FasterProjectiles>(); };
        removeComponentHandler[0] = (skill) => { skill.Remove<FasterProjectiles>(); };

        addComponentHandler[1] = (skill) => { skill.Add<Proliferation>(); };
        removeComponentHandler[1] = (skill) => { skill.Remove<Proliferation>(); };

        addComponentHandler[2] = (skill) => { skill.Add<FasterCast>(); };
        removeComponentHandler[2] = (skill) => { skill.Remove<FasterCast>(); };
    }

    public void MakePool(int parentsIdx, int prefabsIdx)
    {
        for (int i = 0; i < MAX_PREFAB_COUNT; i++)
        {
            var pool = Instantiate(skillPrefabs[prefabsIdx], poolParents[parentsIdx]);
            pool.SetActive(false);

            skillPool[parentsIdx].Enqueue(pool);
        }
    }

    public void RemovePool(int parentsIdx)
    {
        while (skillPool[parentsIdx].Count > 0)
        {
            var obj = skillPool[parentsIdx].Dequeue();
            Destroy(obj);
        }
    }

    public void AddSkillComponent(int parentsIdx, int prefabsIdx, int componentKey)
    {
        //큐 초기화
        RemovePool(parentsIdx);

        // 컴포넌트 추가하기
        for (int i = 0; i < MAX_PREFAB_COUNT; i++)
        {
            var pool = Instantiate(skillPrefabs[prefabsIdx], poolParents[parentsIdx]);
            var tempComponent = pool.GetComponent<Skill>();

            addComponentHandler[componentKey - 300]?.Invoke(tempComponent);

            pool.SetActive(false);
            skillPool[parentsIdx].Enqueue(pool);
        }
    }

    public void RemoveSkillComponent(int parentsIdx, int componentKey)
    {
        // 컴포넌트 제거하기
        for (int i = 0; i < MAX_PREFAB_COUNT; i++)
        {
            var pool = skillPool[parentsIdx].Dequeue();
            var tempComponent = pool.GetComponent<Skill>();

            removeComponentHandler[componentKey - 300]?.Invoke(tempComponent);

            pool.SetActive(false);
            skillPool[parentsIdx].Enqueue(pool);
        }
    }

    public void UseSkill(int idx)
    {
        //todo 스킬 특성 별로 발사 위치 시전 모션 등등 다르게 해보기
        var temp = skillPool[idx].Dequeue();
        temp.transform.position = firePoint.position;
        temp.SetActive(true);
    }

    private void InitializeSkillPoolOnSceneLoad()
    {
        var keySet = GameDataSync.Instance.gemKeySet;
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < keySet[i].Count; j++)
            {
                if (keySet[i][j] >= 200 && keySet[i][j] < 300)
                {
                    MakePool(i, keySet[i][j] - 200);
                }
                else
                {
                    AddSkillComponent(i, keySet[i][0]-200, keySet[i][j]);
                }
            }
        } 
    }
}