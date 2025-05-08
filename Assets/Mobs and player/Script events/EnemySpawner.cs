using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Варианты врагов")]
    public GameObject[] enemyPrefabs;

    [Header("Радиусы спавна")]
    public float innerRadius = 5f;
    public float outerRadius = 20f;

    [Header("Настройки волны")]
    public float spawnInterval = 2f;         // Интервал между спавнами
    public int enemiesPerSpawn = 1;           // Сколько врагов спавнить за раз
    public int enemiesPerWave = 20;           // Сколько врагов в первой волне
    public int waveEnemyIncrease = 5;         // На сколько увеличивать врагов с каждой волной
    private int totalWaves;                // Сколько всего волн
    public float breakBetweenWaves = 60f;     // Пауза между волнами

    [Header("Настройки Surrounder")]
    public int surrounderCount = 8;           // Сколько окружателей вокруг игрока
    public float surrounderRadius = 5f;        // Радиус окружения игрока

    [Header("NavMesh Settings")]
    public float navMeshSampleRadius = 2f;

    [Header("Ссылки")]
    public Transform player;

    private int currentWave = 1;
    private int enemiesSpawnedInWave = 0;
    private bool waveActive = true;
    private bool waitingForNextWave = false;
    private float spawnTimer;
    private float breakTimer;
    private List<GameObject> spawnedEnemies = new();

    void Start()
    {
        totalWaves = GameManager.Instance != null ? GameManager.Instance.waveCount : totalWaves;
    }

    void Update()
    {
        if (currentWave > totalWaves)
            return; // Все волны завершены

        if (waveActive)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval && enemiesSpawnedInWave < enemiesPerWave)
            {
                spawnTimer = 0f;
                SpawnEnemies();
            }

            if (enemiesSpawnedInWave >= enemiesPerWave)
            {
                waveActive = false;
                waitingForNextWave = true;
                breakTimer = 0f;
            }
        }
        else if (waitingForNextWave)
        {
            CleanupNullEnemies();

            if (spawnedEnemies.Count == 0)
            {
                breakTimer += Time.deltaTime;
                if (breakTimer >= breakBetweenWaves)
                {
                    StartNextWave();
                }
            }
        }
    }

    void SpawnEnemies()
    {
        int remaining = enemiesPerWave - enemiesSpawnedInWave;
        int spawnCount = Mathf.Min(enemiesPerSpawn, remaining);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition();

            if (spawnPos != Vector3.zero)
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                if (prefab.name.Contains("Surrounder"))
                {
                    SpawnSurrounders(prefab);
                }
                else
                {
                    GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                    spawnedEnemies.Add(newEnemy);
                    enemiesSpawnedInWave++;
                }
            }
        }
    }

    void SpawnSurrounders(GameObject surrounderPrefab)
    {
        for (int j = 0; j < surrounderCount; j++)
        {
            float angle = j * Mathf.PI * 2f / surrounderCount;
            Vector3 spawnOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * surrounderRadius;
            Vector3 spawnPos = player.position + spawnOffset;

            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, navMeshSampleRadius, NavMesh.AllAreas))
            {
                GameObject newEnemy = Instantiate(surrounderPrefab, hit.position, Quaternion.identity);
                spawnedEnemies.Add(newEnemy);
                enemiesSpawnedInWave++;
            }
        }
    }

    void StartNextWave()
    {
        currentWave++;
        if (currentWave <= totalWaves)
        {
            waveActive = true;
            waitingForNextWave = false;
            enemiesSpawnedInWave = 0;
            spawnTimer = 0f;
            enemiesPerWave += waveEnemyIncrease; // Увеличиваем количество врагов с каждой волной
            Debug.Log($"Старт волны {currentWave} с {enemiesPerWave} врагами");
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int i = 0; i < 30; i++)
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

    public int TotalEnemiesRemaining()
{
    CleanupNullEnemies();
    return spawnedEnemies.Count;
}

public bool IsAllWavesFinished()
{
    return currentWave > totalWaves;
}


}
