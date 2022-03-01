using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEndReloadShotgun : MonoBehaviour
{
    public ShotgunController shotgunController;
    private static readonly int State = Animator.StringToHash("State");

    public void Start()
    {
        shotgunController = GetComponent<ShotgunController>();
    }

    public void AddAmmo()
    {
        shotgunController.weaponObject.ammoAmount += 1;
        if (shotgunController.weaponObject.ammoAmount < shotgunController.weaponObject.maxAmmoAmount)
        {
            shotgunController.animator.SetInteger(State, 3);
            shotgunController.amIReloading = true;
        }
        else
        {
            shotgunController.amIReloading = false;
        }
    }
}
