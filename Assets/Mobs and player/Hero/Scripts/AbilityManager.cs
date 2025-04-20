using UnityEngine;
using System.IO;

[System.Serializable]
public class KeybindsData
{
    public string ability1Key = "Alpha1";
    public string ability2Key = "Alpha2";
    public string ability3Key = "Alpha3";
}

public class AbilityManager : MonoBehaviour
{
    private AbilityOne abilityOne;
    private AbilityTwo abilityTwo;
    private AbilityThree abilityThree;

    private KeyCode ability1Key;
    private KeyCode ability2Key;
    private KeyCode ability3Key;

    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "keybinds.json");

        abilityOne = GetComponent<AbilityOne>();
        abilityTwo = GetComponent<AbilityTwo>();
        abilityThree = GetComponent<AbilityThree>();

        LoadKeybinds();
    }

    void Update()
    {
        if (Input.GetKeyDown(ability1Key)) abilityOne.UseAbility();
        if (Input.GetKeyDown(ability2Key)) abilityTwo.UseAbility();
        if (Input.GetKeyDown(ability3Key)) abilityThree.UseAbility();
    }

    public void SetAbilityKey(int abilityNumber, KeyCode newKey)
    {
        switch (abilityNumber)
        {
            case 1: ability1Key = newKey; break;
            case 2: ability2Key = newKey; break;
            case 3: ability3Key = newKey; break;
        }
        SaveKeybinds();
    }

    private void LoadKeybinds()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            KeybindsData data = JsonUtility.FromJson<KeybindsData>(json);

            ability1Key = ParseKey(data.ability1Key, KeyCode.Alpha1);
            ability2Key = ParseKey(data.ability2Key, KeyCode.Alpha2);
            ability3Key = ParseKey(data.ability3Key, KeyCode.Alpha3);
        }
        else
        {
            ability1Key = KeyCode.Alpha1;
            ability2Key = KeyCode.Alpha2;
            ability3Key = KeyCode.Alpha3;
        }
    }

    private void SaveKeybinds()
    {
        KeybindsData data = new KeybindsData
        {
            ability1Key = ability1Key.ToString(),
            ability2Key = ability2Key.ToString(),
            ability3Key = ability3Key.ToString()
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    private KeyCode ParseKey(string key, KeyCode fallback)
    {
        return System.Enum.TryParse(key, out KeyCode parsed) ? parsed : fallback;
    }
}