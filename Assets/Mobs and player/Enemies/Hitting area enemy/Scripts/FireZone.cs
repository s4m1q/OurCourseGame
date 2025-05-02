using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float damagePerSecond = 5f;
    public float duration = 3f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
