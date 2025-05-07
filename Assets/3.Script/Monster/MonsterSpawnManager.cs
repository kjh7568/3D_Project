using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform monsterParent;
    [SerializeField] private Transform[] spawnPointsArray;
    [SerializeField] private GameObject[] monsterPrefabs;

    private List<Transform> spawnPoints = new();

    private void Start()
    {
        SetSpawnPoints();
        SpawnMonster();
    }

    private void SetSpawnPoints()
    {
        for (int i = 0; i < spawnPointsArray.Length / 2; i++)
        {
            int idx = Random.Range(0, spawnPointsArray.Length);


            if (spawnPointsArray[idx].gameObject.activeSelf)
            {
                i--;
                continue;
            }

            spawnPointsArray[idx].gameObject.SetActive(true);
            spawnPoints.Add(spawnPointsArray[idx]);
        }
    }

    private int SpawnRate()
    {
        var rand = Random.Range(0f, 1f);
        if (rand < 0.7f)
        {
            return 0;
        }
        else if (rand < 1f)
        {
            return 1;
        }

        return -1;
    }

    private void SpawnMonster()
    {
        foreach (var center in spawnPoints)
        {
            int spawnCount = Random.Range(3, 8); // 3~7마리

            for (int i = 0; i < spawnCount; i++)
            {
                int monsterIndex = SpawnRate();
                // 원형 범위 내 랜덤 위치 계산 (직경 5 → 반지름 2.5)
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 2.5f;
                Vector3 spawnPosition = center.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

                Instantiate(monsterPrefabs[monsterIndex], spawnPosition, Quaternion.identity, monsterParent);
            }
        }
    }
}