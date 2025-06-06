using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerDeathHandler : MonoBehaviour
{
    public PlayerController player;
    public AbilityOne abilityOne;
    public AbilityTwo abilityTwo;
    public AbilityThree abilityThree;
    public combat meleeAttack;

    public Transform respawnPoint;
    public GameObject gameOverUI; // Панель с надписью "Game Over"
    public float delayBeforeReturn = 3f;

    private bool isDead = false;

    void Update()
    {
        if (!isDead && player.currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        isDead = true;

        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "Level 1") // замените на имя вашей первой сцены
        {
            RespawnPlayer() ;
        }
        else
        {
            GameOver();
        }
    }
    protected virtual string GetCurrentSceneName() => SceneManager.GetActiveScene().name;
    protected virtual void RespawnPlayer() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    /*
    void RespawnPlayer()
    {   
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    */
    

    protected virtual void GameOver()
    {
        
        
        gameOverUI.SetActive(true);
        

        Invoke("ReturnToMainMenu", delayBeforeReturn);
    }

    protected virtual void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Название сцены главного меню
    }


    
}
