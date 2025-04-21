using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Coin";
    public int value = 1;

    public float attractionRadius = 3f;
    public float attractionSpeed = 5f;
    public float rotationSpeed = 360f; // Скорость вращения

    private Transform player;
    private TrailRenderer trail; // Ссылка на TrailRenderer

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.enabled = false; // Выключаем след по умолчанию
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attractionRadius)
            {
                // Включаем след, если в радиусе
                if (trail != null && !trail.enabled)
                    trail.enabled = true;

                // Притягиваемся к игроку
                transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);

                // Вращаемся
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player picked up {itemName}");

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AddScore(value);
            }

            Destroy(gameObject);
        }
    }
}
