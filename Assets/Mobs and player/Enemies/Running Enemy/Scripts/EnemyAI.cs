using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // Параметры врага
    public float maxHealth = 100f; // Максимальное здоровье
    public float currentHealth;   // Текущее здоровье
    public float damage = 10f;    // Урон, который наносит враг
    public float fieldOfViewRadius = 5f; // Радиус области видимости
    public float dropChance = 0.5f;
    public GameObject dropPrefab;
    // Ссылка на игрока
    public Transform player;
    private bool isPlayerInSight = false; // Флаг, находится ли игрок в зоне видимости

    // Компонент NavMeshAgent для управления движением
    private NavMeshAgent navMeshAgent;

    public float attackRange = 1.5f;

    private Animator animator;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer; 

    public float stunDuration = 0.2f; 

    private float attackCooldown = 1f;
    private float lastAttackTime = -999f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false; 
            navMeshAgent.updateUpAxis = false; 
        }

        if (GameManager.Instance != null)
        {
            maxHealth += GameManager.Instance.enemyHealthMultiplier;
            damage += GameManager.Instance.enemyDamageMultiplier;
        }

        currentHealth = maxHealth;
        isDead = false;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("EnemyAI: Player not found! Make sure the player has the 'Player' tag.");
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("EnemyAI: NavMeshAgent component not found on the enemy!");
        }
    }

    void Update()
    {
        if (isDead || player == null || navMeshAgent == null)
            return;

        CheckPlayerInSight();

        if (isPlayerInSight)
        {
            FollowPlayer();

            if (navMeshAgent.velocity.x > 0.1f)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (navMeshAgent.velocity.x < -0.1f)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                TryAttackPlayer();
            }
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        UpdateAnimation();
    }

    void TryAttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
            Debug.Log("Враг нанёс урон игроку");
            lastAttackTime = Time.time;
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isPlayerInSight && navMeshAgent.velocity.magnitude > 0.1f);
            animator.SetBool("IsDead", isDead);
        }
    }

    void CheckPlayerInSight()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= fieldOfViewRadius)
        {
            isPlayerInSight = true;
        }
        else
        {
            isPlayerInSight = false;
        }
    }

    void FollowPlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Enemy took {damageAmount} damage. Current health: {currentHealth}");

        StartCoroutine(StunAndFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator StunAndFlash()
    {
        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;

        if (animator != null)
            animator.SetBool("IsAttacked", true);

        if (spriteRenderer != null)
            spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        if (animator != null)
            animator.SetBool("IsAttacked", false);

        yield return new WaitForSeconds(stunDuration - 0.1f);

        if (navMeshAgent != null && !isDead)
            navMeshAgent.isStopped = false;
    }

    private void Die()
    {
        AchievementConditions.OnEnemyKilled();
        if (isDead) {
            return;
            }
        isDead = true;
        Debug.Log("Пытаемся запустить анимацию смерти");

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
            Debug.Log("Параметр IsDead установлен в true");
        }

        if (navMeshAgent != null) 
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }

        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDead", true);
        }

        if (dropPrefab != null && Random.value < dropChance)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfViewRadius);
    }
}