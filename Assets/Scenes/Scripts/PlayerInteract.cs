using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactDistance);

            foreach (var hit in hits)
            {
                var statue = hit.GetComponent<StatueInteraction>();
                if (statue != null)
                {
                    statue.Interact();
                    break; // Вышли после первого успешного взаимодействия
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
