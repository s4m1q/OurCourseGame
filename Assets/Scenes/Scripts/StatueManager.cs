using UnityEngine;
using UnityEngine.SceneManagement;

public class StatueManager : MonoBehaviour
{
    public static StatueManager Instance;

    private int totalStatues = 4;
    private int activatedStatues = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterActivation()
    {
        activatedStatues++;
        if (activatedStatues >= totalStatues)
        {
            Debug.Log("All statues activated. Loading next scene...");
            SceneManager.LoadScene("EndCutScene"); // Укажи тут имя нужной сцены
        }
    }
}
