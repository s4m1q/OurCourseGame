using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Варианты врагов")]
    public GameObject[] enemyPrefabs;

    [Header("Радиусы спавна")]
    public float innerRadius = 5f;  // Вне этой зоны
    public float outerRadius = 20f; // До этой зоны

    [Header("Настройки спавна")]
    public float spawnInterval = 10f; // Интервал между волнами
    public int enemiesPerWave = 3;    // Сколько врагов за волну
    public int maxEnemies = 20;       // Максимум одновременно

    [Header("NavMesh Settings")]
    public float navMeshSampleRadius = 2f;

    [Header("Ссылки")]
    public Transform player;

    private float spawnTimer;
    private List<GameObject> spawnedEnemies = new();

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && spawnedEnemies.Count < maxEnemies)
        {
            spawnTimer = 0f;
            SpawnWave();
        }

        CleanupNullEnemies();
    }

    void SpawnWave()
    {
        int enemiesToSpawn = Mathf.Min(enemiesPerWave, maxEnemies - spawnedEnemies.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition();

            if (spawnPos != Vector3.zero)
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(newEnemy);
            }
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int i = 0; i < 30; i++) // 30 попыток найти точку
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float dist = Random.Range(innerRadius, outerRadius);

            Vector3 candidate = player.position + new Vector3(dir.x, dir.y, 0f) * dist;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, navMeshSampleRadius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }

    void CleanupNullEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }
}
