using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    public bool Enabled { get; private set; } = true;

    public FPSController controller;
    public EntityComponent entityComponent;
    public HandsController handsController;

    public void Disable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        controller.sensitivity = 0;
        handsController.hands.GetComponent<WeaponController>().isActive = false;
    }

    public void Enable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller.sensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
        handsController.hands.GetComponent<WeaponController>().isActive = true;
    }
}
