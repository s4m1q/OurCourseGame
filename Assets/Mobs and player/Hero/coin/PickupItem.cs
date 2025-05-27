using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    public int value = 50;

    public int minRange=0;

    public int maxRange=100;

    public float attractionRadius = 3f;
    public float attractionSpeed = 5f;
    public float rotationSpeed = 360f;

    private Transform player;
    private TrailRenderer trail; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.enabled = false; 
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attractionRadius)
            {
                if (trail != null && !trail.enabled)
                    trail.enabled = true;

                transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);

                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(itemName=="Coin"){
                Debug.Log($"Player picked up {itemName}");

                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.AddScore(value+Random.Range(minRange,maxRange));
                }
                Destroy(gameObject);
            }
            if(itemName=="HP"){
                Debug.Log($"Player picked up {itemName}");
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.AddHP(value+Random.Range(minRange,maxRange));
                }
                Destroy(gameObject);
            }
            if(itemName=="Stamina"){
                Debug.Log($"Player picked up {itemName}");
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.AddStamina(value+Random.Range(minRange,maxRange));
                }
                Destroy(gameObject);
            }
            
        }
    }
}
