using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel = 1;
    public float enemyHealthMultiplier = 20f;
    public float enemyDamageMultiplier = 5f;
    public int waveCount = 4;

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
        // Увеличиваем сложность при загрузке новой сцены
        if (scene.buildIndex != 0) // допустим, 0 — это главное меню
        {
            currentLevel++;
            enemyHealthMultiplier *= 1.2f;     
            enemyDamageMultiplier *= 1.15f;
            waveCount += 1;
        }
    }
}