using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject settingsPanel; // Ссылка на панель настроек
    public GameObject MenuPanel;

    public void PlayGame()
    {
        // Здесь можно загрузить сцену игры
        SceneManager.LoadScene("IntroScene");
    }

    public void OpenSettings()
    {
        // Включение панели настроек
        MenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ViewProgress()
    {
        // Здесь можно открыть меню прогресса
        Debug.Log("Viewing Progress...");
    }

    public void ExitGame()
    {
        // Здесь можно выйти из игры
        Application.Quit();
    }

    public void CloseSettings()
    {
        // Выключение панели настроек
        settingsPanel.SetActive(false);
    }
}
