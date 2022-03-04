using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityPanelController : MonoBehaviour
{
    public AbilityComponent abilityComponent;
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        string str = "Постоянные эффекты:\n";
        for (var i = 0; i < abilityComponent.Abilities.Length; i++)
        {
            if (float.IsPositiveInfinity(abilityComponent.Abilities[i].Duration))
            {
                str += $"> {abilityComponent.Abilities[i].Name}\n{abilityComponent.Abilities[i].Description}\n";
            }
        }

        str += "\nВременные эффекты:";
        for (var i = 0; i < abilityComponent.Abilities.Length; i++)
        {
            if (float.IsPositiveInfinity(abilityComponent.Abilities[i].Duration))
            {
                str += $"> {abilityComponent.Abilities[i].Name} - {(int)abilityComponent.Abilities[i].DurationRemaining}\n{abilityComponent.Abilities[i].Description}\n";
            }
        }
        text.text = str;
    }
}
