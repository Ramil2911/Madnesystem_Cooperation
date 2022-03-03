using System;
using UnityEngine;

public class AddRandomAbility : MonoBehaviour
{
    public int[] ids;
    public AbilityComponent abilityComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < ids.Length; ++i)
        {
            abilityComponent.Add((Ability)Activator.CreateInstance(Ability.GetById(ids[i])));
        }
    }
}
