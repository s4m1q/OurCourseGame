using UnityEngine;

public class SpikeLogic : MonoBehaviour
{
    private bool isOnSpikes = false;
    private float spikeDamageTimer = 0f;
    public float spikeDamageInterval = 1f; // Интервал между уронами
    public float spikeDamageAmount = 10f;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("SpikeLogic: PlayerController не найден на объекте!");
        }
    }

    void Update()
    {
        if (isOnSpikes)
        {
            spikeDamageTimer += Time.deltaTime;
            //Debug.Log($"Spike damage timer: {spikeDamageTimer}");

            if (spikeDamageTimer >= spikeDamageInterval)
            {
                if (playerController != null)
                {
                    playerController.TakeDamage(spikeDamageAmount);
                }
                spikeDamageTimer = 0f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("spike"))
        {
            isOnSpikes = true;
            Debug.Log("Player is on spikes.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("spike"))
        {
            isOnSpikes = false;
            Debug.Log("Player left spikes.");
        }
    }
}