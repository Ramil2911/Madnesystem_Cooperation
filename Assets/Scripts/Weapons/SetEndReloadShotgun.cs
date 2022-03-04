using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEndReloadShotgun : MonoBehaviour
{
    public WeaponController weaponController;
    private static readonly int State = Animator.StringToHash("State");

    public void Start()
    {
        weaponController = GetComponent<WeaponController>();
    }

    public void AddAmmo()
    {
        weaponController.weaponObject.ammoAmount += 1;
        if (weaponController.weaponObject.ammoAmount < weaponController.weaponObject.maxAmmoAmount-1)
        {
            weaponController.animator.SetInteger(State, 3);
            weaponController.amIReloading = true;
        }
        else
        {
            weaponController.amIReloading = false;
        }
    }
}
