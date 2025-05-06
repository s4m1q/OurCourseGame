using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsManager : MonoBehaviour
{
    public InputField ability1Input;
    public InputField ability2Input;
    public InputField ability3Input;
    public Button applyButton;
    public Slider volumeSlider; // добавляем слайдер громкости

    private AbilityManager abilityManager;

    void Start()
    {
        abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();

        string filePath = System.IO.Path.Combine(Application.persistentDataPath, "keybinds.json");

        string default1 = "1", default2 = "2", default3 = "3";

        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            KeybindsData data = JsonUtility.FromJson<KeybindsData>(json);
            default1 = MapKeyCodeToInputChar(data.ability1Key);
            default2 = MapKeyCodeToInputChar(data.ability2Key);
            default3 = MapKeyCodeToInputChar(data.ability3Key);
        }

        ability1Input.text = default1;
        ability2Input.text = default2;
        ability3Input.text = default3;

        ability1Input.onValueChanged.AddListener(ValidateInput);
        ability2Input.onValueChanged.AddListener(ValidateInput);
        ability3Input.onValueChanged.AddListener(ValidateInput);

        applyButton.onClick.AddListener(SaveSettings);

        // Настройка слайдера громкости
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f); // загрузка
            AudioListener.volume = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value); // сохраняем громкость
        PlayerPrefs.Save();
    }

    void ValidateInput(string input)
    {
        if (input.Length > 1 || !IsValidCharacter(input))
        {
            InputField currentField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
            currentField.text = "";
        }
    }

    bool IsValidCharacter(string input)
    {
        return input.Length == 1 && char.IsLetterOrDigit(input[0]);
    }

    string MapKeyCodeToInputChar(string keyCodeStr)
    {
        if (keyCodeStr.StartsWith("Alpha"))
            return keyCodeStr.Substring(5);
        return keyCodeStr.ToLower();
    }

    KeyCode GetKeyCodeFromInput(string input)
    {
        if (char.IsDigit(input[0]))
            return (KeyCode)Enum.Parse(typeof(KeyCode), "Alpha" + input);
        if (char.IsLetter(input[0]))
            return (KeyCode)Enum.Parse(typeof(KeyCode), input.ToUpper());
        return KeyCode.None;
    }

    public void SaveSettings()
    {
        KeyCode key1 = GetKeyCodeFromInput(ability1Input.text);
        KeyCode key2 = GetKeyCodeFromInput(ability2Input.text);
        KeyCode key3 = GetKeyCodeFromInput(ability3Input.text);

        if (key1 != KeyCode.None) abilityManager.SetAbilityKey(1, key1);
        if (key2 != KeyCode.None) abilityManager.SetAbilityKey(2, key2);
        if (key3 != KeyCode.None) abilityManager.SetAbilityKey(3, key3);

        Debug.Log("Изменения сохранены и применены (JSON)");
    }
}
