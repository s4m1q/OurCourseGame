using UnityEngine;
using UnityEngine.UI;
using TMPro; // Не забудь подключить TextMeshPro

public class AbilityUpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;

    [Header("Способность 1")]
    public Button abilityOneUpgradeButton;
    public Text abilityOneLevelText;
    public TextMeshProUGUI abilityOneCostText;
    public AbilityOne abilityOne;

    [Header("Способность 2")]
    public Button abilityTwoUpgradeButton;
    public Text abilityTwoLevelText;
    public TextMeshProUGUI abilityTwoCostText;
    public AbilityTwo abilityTwo;

    [Header("Способность 3")]
    public Button abilityThreeUpgradeButton;
    public Text abilityThreeLevelText;
    public TextMeshProUGUI abilityThreeCostText;
    public AbilityThree abilityThree;

    public PlayerController player;

    // Массив цен по уровням (индекс соответствует уровню: 1 => 150, 2 => 250 и т.д.)
    private int[] upgradePrices = { 150, 250, 400, 600, 750, 1000 };

    private bool isPanelOpen = false;

    void Start()
    {
        upgradePanel.SetActive(false);
        abilityOneUpgradeButton.onClick.AddListener(UpgradeAbilityOne);
        abilityTwoUpgradeButton.onClick.AddListener(UpgradeAbilityTwo);
        abilityThreeUpgradeButton.onClick.AddListener(UpgradeAbilityThree);
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
        if (CanUpgrade(abilityOne.CurrentLevel))
        {
            player.Coins -= upgradePrices[abilityOne.CurrentLevel];
            abilityOne.CurrentLevel++;
            UpdateUI();
        }
    }

    void UpgradeAbilityTwo()
    {
        if (CanUpgrade(abilityTwo.CurrentLevel))
        {
            player.Coins -= upgradePrices[abilityTwo.CurrentLevel];
            abilityTwo.CurrentLevel++;
            UpdateUI();
        }
    }

    void UpgradeAbilityThree()
    {
        if (CanUpgrade(abilityThree.CurrentLevel))
        {
            player.Coins -= upgradePrices[abilityThree.CurrentLevel];
            abilityThree.CurrentLevel++;
            UpdateUI();
        }
    }

    bool CanUpgrade(int currentLevel)
    {
        return currentLevel < upgradePrices.Length && player.Coins >= upgradePrices[currentLevel];
    }

    void UpdateUI()
    {
        abilityOneLevelText.text = $"Уровень: {abilityOne.CurrentLevel}";
        abilityTwoLevelText.text = $"Уровень: {abilityTwo.CurrentLevel}";
        abilityThreeLevelText.text = $"Уровень: {abilityThree.CurrentLevel}";

        abilityOneCostText.text = GetCostText(abilityOne.CurrentLevel);
        abilityTwoCostText.text = GetCostText(abilityTwo.CurrentLevel);
        abilityThreeCostText.text = GetCostText(abilityThree.CurrentLevel);
    }

    string GetCostText(int currentLevel)
    {
        if (currentLevel < upgradePrices.Length)
        {
            return $"Цена: {upgradePrices[currentLevel]}";
        }
        else
        {
            return "Макс. уровень";
        }
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
