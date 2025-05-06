using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Button displayModeButton;
    public AudioSource audioSource;

    void Start()
    {
        // Инициализация настроек
        settingsPanel.SetActive(false);
        volumeSlider.value = AudioListener.volume;
        //UpdateDisplayModeButtonText();

        // Добавление обработчиков событий
        volumeSlider.onValueChanged.AddListener(SetVolume);
        displayModeButton.onClick.AddListener(ToggleDisplayMode);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ToggleDisplayMode()
    {
        if (Screen.fullScreen)
        {
            // Переключение в оконный режим
            Screen.fullScreen = false;
            Screen.SetResolution(1280, 720, false);
        }
        else
        {
            // Переключение в полноэкранный режим
            Screen.fullScreen = true;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }

        // Обновление текста на кнопке
        UpdateDisplayModeButtonText();
    }

    private void UpdateDisplayModeButtonText()
    {
        Text buttonText = displayModeButton.GetComponentInChildren<Text>();
        if (Screen.fullScreen)
        {
            buttonText.text = "Windowed Mode";
        }
        else
        {
            buttonText.text = "Full Screen Mode";
        }
    }
}
