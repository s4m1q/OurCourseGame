using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;       // Общий Canvas (изначально выключен)
    public GameObject pausePanel;        // Панель паузы внутри канваса
    public GameObject settingsPanel;     // Панель настроек внутри канваса
    public AbilityUpgradeUI abilityUpgradeUI;  // Ссылка на UI прокачки
    private bool wasUpgradePanelOpenBeforePause = false;

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
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        if (wasUpgradePanelOpenBeforePause && abilityUpgradeUI != null)
        {
            abilityUpgradeUI.ForceOpen();
        }
    }


    public void Pause()
    {
        if (abilityUpgradeUI != null && abilityUpgradeUI.IsPanelOpen())
        {
            // Закрываем магазин и запоминаем, что он был открыт
            abilityUpgradeUI.ForceClose();
            wasUpgradePanelOpenBeforePause = true;
        }
        else
        {
            wasUpgradePanelOpenBeforePause = false;
        }

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