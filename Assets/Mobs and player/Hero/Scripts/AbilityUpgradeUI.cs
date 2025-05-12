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

    [Header("Улучшение ХП и Стамины")]
    public Button healthUpgradeButton;
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI currentMaxHealthText;

    public Button staminaUpgradeButton;
    public TextMeshProUGUI staminaCostText;
    public TextMeshProUGUI currentMaxStaminaText;

    private int healthUpgradeLevel = 0;
    private int staminaUpgradeLevel = 0;

    [Header("Удар с руки")]
    public Button meleeUpgradeButton;
    public Text meleeLevelText;
    public TextMeshProUGUI meleeCostText;
    public combat meleeAttack;
    private int meleeUpgradeLevel = 0;


    // Массив цен по уровням (индекс соответствует уровню: 1 => 150, 2 => 250 и т.д.)
    private int[] upgradePrices = { 150, 250, 400, 600, 750, 1000, 1500, 2000};

    private bool isPanelOpen = false;

    private bool WasNotification1 = false;
    private bool WasNotification2 = false;
    private bool WasNotification3 = false;
    void Start()
    {
        upgradePanel.SetActive(false);
        abilityOneUpgradeButton.onClick.AddListener(UpgradeAbilityOne);
        abilityTwoUpgradeButton.onClick.AddListener(UpgradeAbilityTwo);
        abilityThreeUpgradeButton.onClick.AddListener(UpgradeAbilityThree);
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
        staminaUpgradeButton.onClick.AddListener(UpgradeStamina);
        meleeUpgradeButton.onClick.AddListener(UpgradeMeleeAttack);

        abilityOne.CurrentLevel = PlayerStats.Instance.abilityOneLevel;
        abilityTwo.CurrentLevel = PlayerStats.Instance.abilityTwoLevel;
        abilityThree.CurrentLevel = PlayerStats.Instance.abilityThreeLevel;
        meleeAttack.attackLevel = PlayerStats.Instance.meleeLevel;
        meleeUpgradeLevel = PlayerStats.Instance.meleeLevel;
        player.maxHealth = PlayerStats.Instance.maxHealth;
        healthUpgradeLevel = PlayerStats.Instance.HealthLevel;
        player.maxStamina = PlayerStats.Instance.maxStamina;
        staminaUpgradeLevel = PlayerStats.Instance.StaminaLevel;

        Debug.Log(PlayerStats.Instance.maxHealth);
        player.Coins = PlayerStats.Instance.coins;
        
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPanelOpen = !isPanelOpen;
            upgradePanel.SetActive(isPanelOpen);
            Time.timeScale = isPanelOpen ? 0f : 1f;

            if (abilityThree.CurrentLevel == 8 && !WasNotification3) {
                AchievementConditions.OnUpgradeAbilityToMaxLevel("Max Level of ability Three");
                WasNotification3 = true;
            }
            if (abilityTwo.CurrentLevel == 8 && !WasNotification2) {
                AchievementConditions.OnUpgradeAbilityToMaxLevel("Max Level of ability Two");
                WasNotification2 = true;
            }
            if (abilityOne.CurrentLevel == 8 && !WasNotification1) {
                AchievementConditions.OnUpgradeAbilityToMaxLevel("Max Level of ability One");
                WasNotification1 = true;
            }
            AchievementConditions.OnHealthIncreased(player.maxHealth);
            AchievementConditions.OnStaminaIncreased(player.maxStamina);
        }
    }

    void UpgradeAbilityOne()
    {
        if (CanUpgrade(abilityOne.CurrentLevel))
        {
            player.Coins -= upgradePrices[abilityOne.CurrentLevel];
            abilityOne.CurrentLevel++;
            PlayerStats.Instance.abilityOneLevel = abilityOne.CurrentLevel;
            PlayerStats.Instance.coins = player.Coins;
            UpdateUI();
        }
    }

    void UpgradeMeleeAttack()
    {
        if (meleeUpgradeLevel < upgradePrices.Length && player.Coins >= upgradePrices[meleeUpgradeLevel])
        {
            player.Coins -= upgradePrices[meleeUpgradeLevel];
            meleeUpgradeLevel++;
            PlayerStats.Instance.meleeLevel = meleeUpgradeLevel;
            PlayerStats.Instance.coins = player.Coins;
            UpdateUI();
        }
    }

    void UpgradeHealth()
    {
        if (healthUpgradeLevel < upgradePrices.Length && player.Coins >= upgradePrices[healthUpgradeLevel])
        {
            player.Coins -= upgradePrices[healthUpgradeLevel];
            player.maxHealth += 15f;
            player.currentHealth = player.maxHealth;
            healthUpgradeLevel++;
            PlayerStats.Instance.HealthLevel = healthUpgradeLevel;
            PlayerStats.Instance.maxHealth = player.maxHealth;
            PlayerStats.Instance.coins = player.Coins;
            UpdateUI();
        }
    }

    void UpgradeStamina()
    {
        if (staminaUpgradeLevel < upgradePrices.Length && player.Coins >= upgradePrices[staminaUpgradeLevel])
        {
            player.Coins -= upgradePrices[staminaUpgradeLevel];
            player.maxStamina += 10f;
            player.currentStamina = player.maxStamina;
            staminaUpgradeLevel++;
            PlayerStats.Instance.StaminaLevel = staminaUpgradeLevel;
            PlayerStats.Instance.maxStamina = player.maxStamina;
            //Debug.Log(PlayerStats.Instance.maxStamina);
            PlayerStats.Instance.coins = player.Coins;
            UpdateUI();
        }
    }

    void UpgradeAbilityTwo()
    {
        if (CanUpgrade(abilityTwo.CurrentLevel))
        {
            player.Coins -= upgradePrices[abilityTwo.CurrentLevel];
            abilityTwo.CurrentLevel++;
            PlayerStats.Instance.abilityTwoLevel = abilityTwo.CurrentLevel;
            PlayerStats.Instance.coins = player.Coins;
            UpdateUI();
        }
    }

    void UpgradeAbilityThree()
    {
        if (CanUpgrade(abilityThree.CurrentLevel))
        {
            player.Coins -= upgradePrices[abilityThree.CurrentLevel];
            abilityThree.CurrentLevel++;
            PlayerStats.Instance.abilityThreeLevel = abilityThree.CurrentLevel;
            PlayerStats.Instance.coins = player.Coins;
            UpdateUI();
        }
    }

    bool CanUpgrade(int currentLevel)
    {
        return currentLevel < upgradePrices.Length && player.Coins >= upgradePrices[currentLevel];
    }

    void UpdateUI()
    {
        abilityOneLevelText.text = $"Level: {abilityOne.CurrentLevel}";
        abilityTwoLevelText.text = $"Level: {abilityTwo.CurrentLevel}";
        abilityThreeLevelText.text = $"Level: {abilityThree.CurrentLevel}";

        abilityOneCostText.text = GetCostText(abilityOne.CurrentLevel, "Max Level of ability One");
        abilityTwoCostText.text = GetCostText(abilityTwo.CurrentLevel, "Max Level of ability Two");
        abilityThreeCostText.text = GetCostText(abilityThree.CurrentLevel, "Max Level of ability Three");

        healthCostText.text = GetHPUpgradeCostText(healthUpgradeLevel);
        staminaCostText.text = GetStaminaUpgradeCostText(staminaUpgradeLevel);

        currentMaxHealthText.text = $"Max HP: {player.maxHealth}";
        currentMaxStaminaText.text = $"Max stamina: {player.maxStamina}";

        meleeLevelText.text = $"Level: {meleeUpgradeLevel}";
        meleeCostText.text = GetCostText(meleeUpgradeLevel, "Max Level of combat");

    }

    string GetCostText(int currentLevel, string nameAbility)
    {
        if (currentLevel < upgradePrices.Length)
        {
            return $"Cost: {upgradePrices[currentLevel]}";
        }
        else
        {   
            return "Max. level";
        }
    }

    string GetHPUpgradeCostText(int level)
    {
        if (level < upgradePrices.Length)
            return $"Cost: {upgradePrices[level]}";
        else
            return "Max. level";
    }

    string GetStaminaUpgradeCostText(int level)
    {
        if (level < upgradePrices.Length)
            return $"Cost: {upgradePrices[level]}";
        else
            return "Max. level";
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
