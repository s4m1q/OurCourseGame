using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

[TestFixture]
public class EnemyAITests
{
    private GameObject enemyGO;
    private EnemyAI enemyAI;
    private GameObject playerGO;
    private NavMeshAgent navAgent;

    [SetUp]
    public void Setup()
    {
        // Создаем игрока
        playerGO = new GameObject("Player");
        playerGO.tag = "Player";
        playerGO.AddComponent<PlayerController>();
        playerGO.transform.position = Vector3.zero;

        // Создаем врага
        enemyGO = new GameObject("Enemy");
        enemyAI = enemyGO.AddComponent<EnemyAI>();

        // Добавляем обязательные компоненты
        navAgent = enemyGO.AddComponent<NavMeshAgent>();
        enemyGO.AddComponent<Animator>();
        enemyGO.AddComponent<SpriteRenderer>();
        enemyGO.AddComponent<BoxCollider2D>();
        enemyGO.AddComponent<Rigidbody2D>();

        // Настраиваем параметры через рефлексию
        SetPrivateField(enemyAI, "navMeshAgent", navAgent);
        SetPrivateField(enemyAI, "animator", enemyGO.GetComponent<Animator>());
        SetPrivateField(enemyAI, "spriteRenderer", enemyGO.GetComponent<SpriteRenderer>());

        // Принудительно инициализируем параметры
        SetFieldValue("maxHealth", 100f);
        SetFieldValue("currentHealth", 100f);
        SetFieldValue("damage", 10f);
        SetFieldValue("fieldOfViewRadius", 5f);
        SetFieldValue("attackRange", 1.5f);
        SetFieldValue("player", playerGO.transform);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyGO);
        Object.DestroyImmediate(playerGO);
    }

    private void SetFieldValue(string fieldName, object value)
    {
        var field = typeof(EnemyAI).GetField(fieldName, 
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        field.SetValue(enemyAI, value);
    }

    private T GetPrivateField<T>(string fieldName)
    {
        var field = typeof(EnemyAI).GetField(fieldName, 
            BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)field.GetValue(enemyAI);
    }

    [Test]
    public void TakeDamage_ReducesHealthCorrectly()
    {
        enemyAI.TakeDamage(30f);
        Assert.AreEqual(70f, GetPrivateField<float>("currentHealth"));
    }

    [UnityTest]
    public IEnumerator Die_DisablesComponents()
    {
        enemyAI.TakeDamage(100f);
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(GetPrivateField<bool>("isDead"));
        Assert.IsFalse(enemyGO.GetComponent<BoxCollider2D>().enabled);
        Assert.IsFalse(enemyGO.GetComponent<Rigidbody2D>().simulated);
    }

    [UnityTest]
    public IEnumerator PlayerDetection_WithinRadius()
    {
        playerGO.transform.position = enemyGO.transform.position + Vector3.right * 3f;
        yield return null;

        var checkMethod = typeof(EnemyAI).GetMethod("CheckPlayerInSight", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        checkMethod.Invoke(enemyAI, null);

        Assert.IsTrue(GetPrivateField<bool>("isPlayerInSight"));
    }
}

public class PlayerController : MonoBehaviour
{
    public void TakeDamage(float damage) { }
}