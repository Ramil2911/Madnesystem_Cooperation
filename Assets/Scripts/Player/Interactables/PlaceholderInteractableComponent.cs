using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderInteractableComponent : Interactable
{
    public override void Interact(GameObject actor)
    {
        Debug.Log(actor.name + " interacted with me!");
    }
}
