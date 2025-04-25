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
        for (int i = 0; i < spawnPointsArray.Length/2; i++)
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

    private void SpawnMonster()
    {
        int monsterIndex = Random.Range(0, monsterPrefabs.Length);

        foreach (var center in spawnPoints)
        {
            int spawnCount = Random.Range(3, 6); // 3~5마리

            for (int i = 0; i < spawnCount; i++)
            {
                // 원형 범위 내 랜덤 위치 계산 (직경 5 → 반지름 2.5)
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 2.5f;
                Vector3 spawnPosition = center.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

                Instantiate(monsterPrefabs[monsterIndex], spawnPosition, Quaternion.identity, monsterParent);
            }
        }
    }
}