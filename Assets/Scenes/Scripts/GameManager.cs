using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel = 1;
    public float enemyHealthMultiplier = 0f;
    public float enemyDamageMultiplier = 0f;
    public int waveCount = 4;

    private bool firstLoadDone = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0) // допустим, 0 — это главное меню
        {
            if (firstLoadDone)
            {
                currentLevel++;
                enemyHealthMultiplier += 20f;     
                enemyDamageMultiplier += 5f;
                waveCount += 1;
            }
            else
            {
                firstLoadDone = true; // Первый уровень загружен — больше не трогаем
            }
        }
    }
}