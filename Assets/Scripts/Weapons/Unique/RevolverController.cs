using UnityEngine;
using UnityEngine.UI;
using UniversalMobileController;

public class RevolverController : WeaponController
{
    private void Start()
    {
        FindUI();
        Reload();
    }
    void Update()
    {
        if(!isActive) return;
        ammoText.text = weaponObject.ammoAmount + "/" + weaponObject.maxAmmoAmount;
        RunCommonShootingStuff();
        if (shootWasOneFrameBefore) doIShoot = false;
        shootWasOneFrameBefore = false;
    }

    public override void Shoot()
    {
        
        if(weaponObject.ammoAmount<=0)
        {
            Reload();
            return;
        }
        weaponObject.ammoAmount--;

        animator.SetInteger(State, 2);
        
        vfxController.Shoot();
        
        var transform1 = vfxController.transform;
        var bulletGO = Instantiate(bullet, transform1.position + transform1.forward, transform1.rotation);
        //bulletGO.GetComponent<Rigidbody>().velocity = transform.forward * weaponObject.BulletSpeed;
        var bulletComponent = bulletGO.GetComponent<BulletComponent>();
        bulletComponent.damageAmount = weaponObject.damage*WeaponBuff.damageMultiplier;
        bulletComponent.owner = this.gameObject;
        
        //отдача
        recoilController.Add(impact, impactDuration);
        
        
        //TODO: fix text
//        ammo.text = weaponObject.ammoAmount.ToString() + " / " + weaponObject.maxAmmoAmount;
    }

    public override void Reload()
    {
        if(weaponObject.ammoAmount == weaponObject.maxAmmoAmount) return;
        doIShoot = false;
        
        animator.SetInteger(State,3 );

        //var ammoAmount = inventoryComponent.ammoInventory[weaponObject.requiredAmmoType];
        weaponObject.ammoAmount = weaponObject.maxAmmoAmount;
        /*if (ammoAmount == 0)
            return;
        if (ammoAmount >= weaponObject.MaxAmmoAmount)
        {
            inventoryComponent.ammoInventory[weaponObject.requiredAmmoType] -= (uint)weaponObject.MaxAmmoAmount;
            weaponObject.ammoAmount = weaponObject.MaxAmmoAmount;
        }
        else
        {
            weaponObject.ammoAmount = (int)ammoAmount;
            inventoryComponent.ammoInventory[weaponObject.requiredAmmoType] = 0;
        }*/
        
        //TODO: fix text
        //ammo.text =  weaponObject.ammoAmount.ToString() + " / " + weaponObject.maxAmmoAmount;
    }
}
