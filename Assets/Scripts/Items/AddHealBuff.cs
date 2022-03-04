using System;
using UnityEngine;

public class AddHealBuff : Interactable
{
    public override void Interact(GameObject actor)
    {
        var instance = (Ability)Activator.CreateInstance(Ability.GetById(0));
        actor.GetComponent<AbilityComponent>().Add(instance);
        Destroy(this.gameObject);
    }
}
