using UnityEngine;
using UnityEngine.UI;
using UniversalMobileController;

//TODO: rewrite weapon scripts, they all need common parts to be moved to WeaponController to avoid boilerplate code
public class ShotgunController : WeaponController
{
    public int drobAmount = 7;
    
    private void Start()
    {
        FindUI();
        Reload();
    }

    void Update()
    {
        if(!isActive || amIReloading) return;
        ammoText.text = weaponObject.ammoAmount + "/" + weaponObject.maxAmmoAmount;
        RunCommonShootingStuff();
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
        var bulletComponent = bulletGO.GetComponent<DrobSpawner>();
        bulletComponent.damage = weaponObject.damage*WeaponBuff.damageMultiplier;
        bulletComponent.owner = this.gameObject;
        bulletComponent.bulletAmount = drobAmount;

        //отдача
        recoilController.Add(impact, impactDuration);
    }
    
    public override void Reload()
    {
        if(weaponObject.ammoAmount == weaponObject.maxAmmoAmount) return;
        doIShoot = false;
        
        animator.SetInteger(State,3 );
        amIReloading = true;

        //var ammoAmount = inventoryComponent.ammoInventory[weaponObject.requiredAmmoType];
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
