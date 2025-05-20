using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RangeZonaEnemyAI : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public float fieldOfViewRadius = 10f;
    public float castRange = 6f;
    public float attackCooldown = 2f;

    public GameObject fireballPrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;

    public GameObject dropPrefab;
    public float dropChance = 0.8f;

    public Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float lastAttackTime = -999f;
    public bool isDead = false;

    private NavMeshAgent navMeshAgent;

    public float stunDuration = 0.2f;

    private Vector3 originalScale;

    public GameObject fireZonePrefab;

    public void Start()
    {
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        if (GameManager.Instance != null)
        {
            maxHealth += GameManager.Instance.enemyHealthMultiplier;
        }

        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("RangedEnemyAI: Player not found!");
        }
    }

    void Update()
    {
        if (isDead || player == null || navMeshAgent == null) return;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        float distance = Vector2.Distance(transform.position, player.position);

        bool seesPlayer = distance <= fieldOfViewRadius;
        bool inAttackRange = distance <= castRange;

        if (seesPlayer)
        {
            FacePlayer();

            if (inAttackRange)
            {
                navMeshAgent.ResetPath();

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    CastFireball();
                }
            }
            else
            {
                Vector3 target = player.position;
                target.z = 0;
                navMeshAgent.SetDestination(target);
            }
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        UpdateAnimation(seesPlayer, inAttackRange);
    }

    public void FacePlayer()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    public void CastFireball()
    {
        if (fireballPrefab == null || firePoint == null || player == null) return;

        Vector3 firePos = firePoint.position;
        firePos.z = 0f;

        Vector3 targetPos = player.position;
        targetPos.z = 0f;

        GameObject fireball = Instantiate(fireballPrefab, firePos, Quaternion.identity);
        Vector2 direction = (targetPos - firePos).normalized;

        FireballZone fb = fireball.GetComponent<FireballZone>(); 
        if (fb != null)
        {
            fb.fireZonePrefab = fireZonePrefab;
            fb.Initialize(direction, targetPos);
        }

        lastAttackTime = Time.time;
    }

    public void UpdateAnimation(bool seesPlayer, bool inAttackRange)
    {
        if (animator == null) return;

        animator.SetBool("IsDead", isDead);
        animator.SetBool("PlayerInRange", seesPlayer);
        animator.SetBool("IsWalking", seesPlayer && !inAttackRange && navMeshAgent.velocity.magnitude > 0.1f);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

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

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
            animator.SetBool("IsWalking", false);
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

        if (dropPrefab != null && Random.value < dropChance)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fieldOfViewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, castRange);
    }
}
