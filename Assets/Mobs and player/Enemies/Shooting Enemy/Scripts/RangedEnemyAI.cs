using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RangedEnemyAI : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public float fieldOfViewRadius = 10f;
    public float castRange = 6f;
    public float attackCooldown = 2f;

    public GameObject fireballPrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;

    public Transform player;
    private Animator animator;
    //private SpriteRenderer spriteRenderer;

    private float lastAttackTime = -999f;
    private bool isDead = false;

    public float stunDuration = 0.2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("RangedEnemyAI: Player not found!");
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Видит игрока?
        bool seesPlayer = distance <= fieldOfViewRadius;
        bool canCast = distance <= castRange;

        if (seesPlayer)
        {
            FacePlayer();
        }

        if (canCast && Time.time - lastAttackTime >= attackCooldown)
        {
            CastFireball();
        }

        UpdateAnimation(seesPlayer, canCast);
    }

    void FacePlayer()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    void CastFireball()
    {
        animator?.SetTrigger("Cast");

        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - firePoint.position).normalized;

            Fireball fb = fireball.GetComponent<Fireball>();
            if (fb != null)
            {
                fb.Initialize(direction);
            }
        }

        lastAttackTime = Time.time;
    }

    void UpdateAnimation(bool seesPlayer, bool canCast)
    {
        animator?.SetBool("IsDead", isDead);
        animator?.SetBool("PlayerInRange", seesPlayer);
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
        if (animator != null)
            animator.SetBool("IsAttacked", true);

        //spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //spriteRenderer.color = Color.white;

        if (animator != null)
            animator.SetBool("IsAttacked", false);

        yield return new WaitForSeconds(stunDuration - 0.1f);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }

        GetComponent<Collider2D>().enabled = false;
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
