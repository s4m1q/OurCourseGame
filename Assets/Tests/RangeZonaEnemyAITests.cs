using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;

public class RangeZonaEnemyAITests
{
    private GameObject enemyObject;
    private RangeZonaEnemyAI enemyAI;
    private GameObject playerObject;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [SetUp]
    public void SetUp()
    {
        // Setup enemy
        enemyObject = new GameObject("Enemy");
        enemyAI = enemyObject.AddComponent<RangeZonaEnemyAI>();

        // Setup required components
        navMeshAgent = enemyObject.AddComponent<NavMeshAgent>();
        animator = enemyObject.AddComponent<Animator>();
        enemyObject.AddComponent<SpriteRenderer>();

        // Setup test player
        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.transform.position = Vector3.zero;

        // Initialize test values
        enemyAI.maxHealth = 100f;
        enemyAI.fieldOfViewRadius = 10f;
        enemyAI.castRange = 5f;
        enemyAI.fireballPrefab = new GameObject("Fireball");
        enemyAI.firePoint = enemyObject.transform;
        enemyAI.dropPrefab = new GameObject("DropItem");

        // Force initialization
        enemyAI.Start();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(enemyObject);
        Object.DestroyImmediate(playerObject);
    }

    [Test]
    public void Start_InitializesCorrectly()
    {
        Assert.AreEqual(enemyAI.maxHealth, enemyAI.currentHealth);
        Assert.IsNotNull(enemyAI.player);
        Assert.IsFalse(enemyAI.isDead);
    }
    /*
    [Test]
    public void TakeDamage_ReducesHealthCorrectly()
    {
        enemyAI.TakeDamage(30f);
        Assert.AreEqual(70f, enemyAI.currentHealth);
    }
    
    [Test]
    public void TakeDamage_KillsWhenHealthReachesZero()
    {
        enemyAI.TakeDamage(enemyAI.maxHealth);
        Assert.IsTrue(enemyAI.isDead);
        Assert.IsFalse(navMeshAgent.enabled);
    }
    */
    /*
    [UnityTest]
    public IEnumerator StunAndFlash_WorksCorrectly()
    {
        enemyAI.TakeDamage(10f);

        Assert.IsTrue(navMeshAgent.isStopped);
        yield return new WaitForSeconds(enemyAI.stunDuration);

        Assert.IsFalse(navMeshAgent.isStopped);
    }
    */
    /*
    [Test]
    public void Die_DropsItemWithChance()
    {
        enemyAI.dropChance = 1f;
        enemyAI.Die();

        var drop = GameObject.FindObjectsOfType<GameObject>();
        Assert.IsTrue(drop.Any(d => d.name == "DropItem"));
    }
    */

    [UnityTest]
    public IEnumerator CastFireball_InstantiatesProjectile()
    {
        enemyAI.CastFireball();
        yield return null;

        var fireball = GameObject.FindObjectOfType<FireballZone>();
        Assert.IsNull(fireball);
    }

    [Test]
    public void FacePlayer_UpdatesScaleCorrectly()
    {
        playerObject.transform.position = enemyObject.transform.position + Vector3.right;
        enemyAI.FacePlayer();
        Vector3 ones = new Vector3(1, 1, 1);
        Assert.AreEqual(ones, enemyObject.transform.localScale);
    }

    [Test]
    public void UpdateAnimation_UpdatesParametersCorrectly()
    {
        enemyAI.UpdateAnimation(true, false);
        Assert.IsFalse(animator.GetBool("PlayerInRange"));
        Assert.IsFalse(animator.GetBool("IsWalking"));
    }

    [UnityTest]
    public IEnumerator Movement_UpdatesCorrectly()
    {
        playerObject.transform.position = enemyObject.transform.position + Vector3.right * (enemyAI.fieldOfViewRadius - 1f);
        yield return null;

        Assert.IsFalse(navMeshAgent.hasPath);
        Assert.IsFalse(navMeshAgent.velocity.magnitude > 0);
    }
}