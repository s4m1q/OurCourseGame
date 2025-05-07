using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 15f;
    public float lifeTime = 7f;

    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {

        if (GameManager.Instance != null)
        {
            damage += GameManager.Instance.enemyDamageMultiplier;
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Проверяем, есть ли под фаерболом навмеш
        if (!IsOnNavMesh(transform.position))
        {
            Destroy(gameObject); // Уничтожить, если вне навмеша
        }
    }

    bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        // radius = 0.1f, чтобы проверить точку
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

            Destroy(gameObject); // уничтожить при попадании
        }
    }
}
