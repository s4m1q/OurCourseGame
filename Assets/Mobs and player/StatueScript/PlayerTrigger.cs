using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private bool Epressed = false;
    private bool canPress = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!Epressed)
            {
                text.SetActive(true);
                canPress = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.SetActive(false);
            canPress = false;
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.E) && canPress)
        {
            Epressed = true;
            text.SetActive(false);
        }
    }


}
