using UnityEngine;

public class StatueInteraction : MonoBehaviour
{
    public bool hasBeenActivated = false;

    public void Interact()
    {
        if (!hasBeenActivated)
        {
            hasBeenActivated = true;
            Debug.Log("Statue activated: " + gameObject.name);
            StatueManager.Instance.RegisterActivation();
        }
    }
}
