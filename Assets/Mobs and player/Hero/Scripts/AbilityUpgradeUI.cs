using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class AbilityTwoLevelData
{
    public float HealthRestore;
    public float StaminaRestore;
    public float Cooldown;
}

public class AbilityUpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;

    [Header("Способность 1")]
    public Button abilityOneUpgradeButton;
    public Text abilityOneLevelText;
    public AbilityOne abilityOne;

    [Header("Способность 2")]
    public Button abilityTwoUpgradeButton;
    public Text abilityTwoLevelText;
    public AbilityTwo abilityTwo;

    public PlayerController player;
    public int upgradeCost = 100;

    private bool isPanelOpen = false;

    // Данные уровней для способности 2
    public List<AbilityTwoLevelData> abilityTwoLevels = new List<AbilityTwoLevelData>();
    private int abilityTwoLevel = 1;

    void Start()
    {
        upgradePanel.SetActive(false);
        abilityOneUpgradeButton.onClick.AddListener(UpgradeAbilityOne);
        abilityTwoUpgradeButton.onClick.AddListener(UpgradeAbilityTwo);
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPanelOpen = !isPanelOpen;
            upgradePanel.SetActive(isPanelOpen);
            Time.timeScale = isPanelOpen ? 0f : 1f;
        }
    }

    void UpgradeAbilityOne()
    {
        if (abilityOne.CurrentLevel < 5 && player.Coins >= upgradeCost)
        {
            abilityOne.CurrentLevel++;
            player.Coins -= upgradeCost;
            UpdateUI();
        }
    }

    void UpgradeAbilityTwo()
    {
        if (abilityTwoLevel < 5 && player.Coins >= upgradeCost)
        {
            abilityTwoLevel++;
            player.Coins -= upgradeCost;
            ApplyAbilityTwoLevel();
            UpdateUI();
        }
    }

    void ApplyAbilityTwoLevel()
    {
        var levelData = abilityTwoLevels[Mathf.Clamp(abilityTwoLevel - 1, 0, abilityTwoLevels.Count - 1)];
        abilityTwo.healthRestore = levelData.HealthRestore;
        abilityTwo.staminaRestore = levelData.StaminaRestore;
        abilityTwo.cooldown = levelData.Cooldown;
    }

    void UpdateUI()
    {
        abilityOneLevelText.text = $"Уровень: {abilityOne.CurrentLevel}";
        abilityTwoLevelText.text = $"Уровень: {abilityTwoLevel}";
    }

    public void ForceOpen()
    {
        isPanelOpen = true;
        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ForceClose()
    {
        isPanelOpen = false;
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public bool IsPanelOpen()
    {
        return isPanelOpen;
    }
}
