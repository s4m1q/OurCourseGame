using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerController player;

    [Header("UI Text Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI coinText;

    [Header("UI Backgrounds")]
    public Image healthBox;
    public Image staminaBox;
    public Image coinBox; // Прямоугольник/рамка под монетки

    void Update()
    {
        if (player == null) return;

        // Обновление текста
        healthText.text = $"{Mathf.CeilToInt(player.currentHealth)}/{Mathf.CeilToInt(player.maxHealth)}";
        staminaText.text = $"{Mathf.CeilToInt(player.currentStamina)}/{Mathf.CeilToInt(player.maxStamina)}";
        coinText.text = player.Coins.ToString();

        // Убедимся, что рамки отображаются
        if (!healthBox.enabled) healthBox.enabled = true;
        if (!staminaBox.enabled) staminaBox.enabled = true;
        if (!coinBox.enabled) coinBox.enabled = true;
    }
}
