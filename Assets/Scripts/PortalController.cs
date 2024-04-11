using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    [Header("Ennemis spawnnant du portail")]
    [SerializeField] private List<MonstersData> enemyTypes;
    [Header("Frequence d'apparition des ennemis")]
    [SerializeField] private float spawnInterval;
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyTypes.Count == 0)
            return;

        int randomIndex = Random.Range(0, enemyTypes.Count);
        GameObject enemyPrefab = enemyTypes[randomIndex].monsterPrefab;
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
