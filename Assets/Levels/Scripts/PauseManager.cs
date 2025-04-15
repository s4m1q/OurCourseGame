using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;       // Общий Canvas (изначально выключен)
    public GameObject pausePanel;        // Панель паузы внутри канваса
    public GameObject settingsPanel;     // Панель настроек внутри канваса
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                // Если в настройках — выходим обратно в паузу
                OpenPausePanel();
            }
            else if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true);
        OpenPausePanel();
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenPausePanel()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
