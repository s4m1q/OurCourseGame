using UnityEngine;
using UnityEngine.AI;
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using UnityEngine.TestTools;
using System.Collections.Generic;

public class EnemySpawnerTests
{
    private GameObject spawnerObject;
    private EnemySpawner spawner;
    private GameObject playerObject;
    private GameObject gameManagerObject;

    [SetUp]
    public void Setup()
    {
        // Setup GameManager
        gameManagerObject = new GameObject("GameManager");
        GameManager gameManager = gameManagerObject.AddComponent<GameManager>();
        gameManager.waveCount = 3;

        // Setup player
        playerObject = new GameObject("Player");
        playerObject.transform.position = Vector3.zero;

        // Setup NavMesh
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = Vector3.zero;
        

        // Setup spawner
        spawnerObject = new GameObject("Spawner");
        spawner = spawnerObject.AddComponent<EnemySpawner>();
        spawner.player = playerObject.transform;
        spawner.enemyPrefabs = new GameObject[1];
        spawner.enemyPrefabs[0] = new GameObject("BasicEnemy");

        // Initialize spawner
        spawner.InvokePrivateMethod("Start");
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(spawnerObject);
        Object.DestroyImmediate(playerObject);
        Object.DestroyImmediate(gameManagerObject);
        NavMesh.RemoveAllNavMeshData();
    }

    [Test]
    public void StartNextWave_IncrementsWaveAndEnemies()
    {
        // Arrange
        int initialWave = spawner.GetPrivateField<int>("currentWave");
        int initialEnemies = spawner.enemiesPerWave;

        // Act
        spawner.InvokePrivateMethod("StartNextWave");

        // Assert
        Assert.AreEqual(initialWave + 1, spawner.GetPrivateField<int>("currentWave"));
        Assert.AreEqual(initialEnemies + spawner.waveEnemyIncrease, spawner.enemiesPerWave);
    }

    [Test]
    public void GetValidSpawnPosition_ReturnsValidPosition()
    {
        // Act
        Vector3 position = spawner.InvokePrivateMethod<Vector3>("GetValidSpawnPosition");

        // Assert
        Assert.IsTrue(position != Vector3.zero);
        Assert.IsTrue(NavMesh.SamplePosition(position, out _, 1f, NavMesh.AllAreas));
    }

    [Test]
    public void CleanupNullEnemies_RemovesDestroyedEnemies()
    {
        // Arrange
        var enemies = spawner.GetPrivateField<List<GameObject>>("spawnedEnemies");
        var enemy = new GameObject("TestEnemy");
        enemies.Add(enemy);
        Object.DestroyImmediate(enemy);

        // Act
        spawner.InvokePrivateMethod("CleanupNullEnemies");

        // Assert
        Assert.AreEqual(0, enemies.Count);
    }

    [UnityTest]
    public IEnumerator WaveProgression_WorksCorrectly()
    {
        // Arrange
        spawner.enemiesPerWave = 2;
        spawner.spawnInterval = 0.1f;
        spawner.breakBetweenWaves = 0.1f;

        // Act - Simulate first wave
        yield return new WaitForSeconds(0.5f);

        // Assert
        Assert.AreEqual(2, spawner.GetPrivateField<int>("enemiesSpawnedInWave"));
        Assert.IsTrue(spawner.GetPrivateField<bool>("waitingForNextWave"));
    }

    [Test]
    public void SpawnSurrounders_CreatesCorrectAmount()
    {
        // Arrange
        var surrounder = new GameObject("Surrounder");
        var originalCount = spawner.GetPrivateField<List<GameObject>>("spawnedEnemies").Count;

        // Act
        spawner.InvokePrivateMethod("SpawnSurrounders", new object[] { surrounder });

        // Assert
        Assert.AreEqual(originalCount + spawner.surrounderCount,
                      spawner.GetPrivateField<List<GameObject>>("spawnedEnemies").Count);
    }
}

// Extension methods for reflection
public static class TestHelpers
{
    public static T GetPrivateField<T>(this object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)field.GetValue(obj);
    }

    public static void InvokePrivateMethod(this object obj, string methodName, object[] parameters = null)
    {
        var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(obj, parameters ?? new object[0]);
    }

    public static T InvokePrivateMethod<T>(this object obj, string methodName, object[] parameters = null)
    {
        var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)method.Invoke(obj, parameters ?? new object[0]);
    }
}