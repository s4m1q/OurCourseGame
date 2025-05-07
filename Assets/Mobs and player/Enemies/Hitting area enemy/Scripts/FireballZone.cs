using UnityEngine;
using UnityEngine.AI;

public class FireballZone : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 15f;
    public float lifeTime = 7f;

    private Vector2 direction;
    private Vector3 targetPosition;

    public GameObject fireZonePrefab;
    private Animator animator;

    public void Initialize(Vector2 dir, Vector3 target)
    {
        direction = dir.normalized;
        targetPosition = target;
        targetPosition.z = 0f;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("IsBurning", true); // 🔥 Запускаем анимацию сразу
        }

        if (GameManager.Instance != null)
        {
            damage += GameManager.Instance.enemyDamageMultiplier;
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Проверка, если вне NavMesh — уничтожить
        if (!IsOnNavMesh(transform.position))
        {
            Destroy(gameObject);
            return;
        }

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            LeaveFireZone();
        }
    }

    bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 0.1f, NavMesh.AllAreas);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            LeaveFireZone();
        }
    }

    void LeaveFireZone()
    {
        if (fireZonePrefab != null)
        {
            Instantiate(fireZonePrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
