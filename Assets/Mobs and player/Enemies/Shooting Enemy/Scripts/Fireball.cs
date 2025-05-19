using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 15f;
    public float lifeTime = 7f;

    private Vector2 direction;
    private Vector3 targetPosition;

    public void Initialize(Vector2 dir, Vector3 target)
    {
        direction = dir.normalized;
        targetPosition = target;
        targetPosition.z = 0f;
    }

    public void Start()
    {

        if (GameManager.Instance != null)
        {
            damage += GameManager.Instance.enemyDamageMultiplier;
        }

        Destroy(gameObject, lifeTime);
    }

    public void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        /* 
        if (!IsOnNavMesh(transform.position))
        {
            Destroy(gameObject); 
        } */

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        } 
    }

    /* bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        // radius = 0.1f
        return NavMesh.SamplePosition(position, out hit, 0.1f, NavMesh.AllAreas);
    } */

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject); 
        }
    }
}
