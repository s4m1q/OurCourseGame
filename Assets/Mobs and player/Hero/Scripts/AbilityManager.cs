using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private AbilityOne abilityOne;
    // private AbilityTwo abilityTwo;
    // private AbilityThree abilityThree;
    // private AbilityQ abilityQ;
    // private AbilityE abilityE;
    // private AbilityR abilityR;

    void Start()
    {
        abilityOne = GetComponent<AbilityOne>();
        // abilityTwo = GetComponent<AbilityTwo>();
        // abilityThree = GetComponent<AbilityThree>();
        // abilityQ = GetComponent<AbilityQ>();
        // abilityE = GetComponent<AbilityE>();
        // abilityR = GetComponent<AbilityR>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilityOne.TryUse();
        }  
        /*
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilityTwo.TryUse();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilityThree.TryUse();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilityQ.TryUse();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            abilityE.TryUse();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            abilityR.TryUse();
        }
        */
    }
}
