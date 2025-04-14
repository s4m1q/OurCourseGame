using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas; // Ссылка на весь Canvas
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false); // Выключаем весь Canvas
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true); // Включаем весь Canvas
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
