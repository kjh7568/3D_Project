using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private const int MAX_SKILL_COUNT = 3;
    private const int MAX_PREFAB_COUNT = 10;

    public List<GameObject> skillPrefabs = new List<GameObject>();
    public Queue<GameObject>[] skillPool = new Queue<GameObject>[MAX_SKILL_COUNT];

    [SerializeField] private Transform[] poolParents = new Transform[MAX_SKILL_COUNT];

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < MAX_SKILL_COUNT; i++)
        {
            skillPool[i] = new Queue<GameObject>();
        }

        MakePool();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            for (int i = 0; i < MAX_PREFAB_COUNT; i++)
            {
                var temp = skillPool[0].Dequeue();
                var tempComponent = temp.GetComponent<Skill>();

                tempComponent.Add<FasterProjectiles>();

                skillPool[0].Enqueue(temp);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            for (int i = 0; i < MAX_PREFAB_COUNT; i++)
            {
                var temp = skillPool[0].Dequeue();
                var tempComponent = temp.GetComponent<Skill>();

                tempComponent.Remove<FasterProjectiles>();

                skillPool[0].Enqueue(temp);
            }
        }
    }

    public void MakePool()
    {
        for (int i = 0; i < MAX_PREFAB_COUNT; i++)
        {
            var pool = Instantiate(skillPrefabs[0], poolParents[0]);
            pool.SetActive(false);

            skillPool[0].Enqueue(pool);
        }
    }

    public void UseSkill(int idx)
    {
        try
        {
            var temp2 = skillPrefabs[idx] == null;
        }
        catch
        {
            Debug.Log("스킬 슬롯에 스킬이 없습니다.");
            return;
        }

        var temp = skillPool[idx].Dequeue();
        temp.transform.position = Player.LocalPlayer.transform.position;
        temp.SetActive(true);
        temp.GetComponent<Skill>().Cast();
    }
}