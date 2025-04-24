using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawnManager : MonoBehaviour
{
    private const int MOB_PACK_COUNT = 9;

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
        for (int i = 0; i < MOB_PACK_COUNT; i++)
        {
            int idx = Random.Range(0, spawnPointsArray.Length);

            if (spawnPointsArray[idx].gameObject.activeSelf)
            {
                i--;
                continue;
            }

            spawnPoints.Add(spawnPointsArray[idx]);
        }
    }

    private void SpawnMonster()
    {
        int monsterIndex = Random.Range(0, monsterPrefabs.Length);

        foreach (var point in spawnPoints)
        {
            Instantiate(monsterPrefabs[monsterIndex], point.position, Quaternion.identity, monsterParent);
        }
    }
}