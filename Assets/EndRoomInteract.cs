using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomInteract : Interactable
{
    public GameObject loader;

    override public void Interact(GameObject actor) {
        // Instantiate(loader).GetComponent<LoadScene>().Load(3);
    }
}
