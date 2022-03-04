using System;
using UnityEngine;

public class AddHealBuff : Interactable
{
    public override void Interact(GameObject actor)
    {
        actor.GetComponent<AbilityComponent>().Add((Ability)Activator.CreateInstance(Ability.GetById(0)));
    }
}
