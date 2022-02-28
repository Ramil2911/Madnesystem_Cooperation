using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Animator animator;
    public WeaponDescription weaponObject;
    public VFXController vfxController;
    
    private static readonly int State = Animator.StringToHash("State");

    public bool doIShoot = false;
    public Text ammo;
    
    public GameObject bullet;

    private void Start() 
    {
        Reload();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if(doIShoot == false)
                animator.SetInteger(State, 1);
        }
        else 
        {
            if(doIShoot == false)
                animator.SetInteger(State, 0);
        }

        if (weaponObject.weaponType == WeaponType.Auto && Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }
        else if (weaponObject.weaponType == WeaponType.SemiAuto && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (doIShoot == false)
            {
                doIShoot = true;
                Shoot();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
    }

    void Shoot()
    {
        
        if(weaponObject.ammoAmount<=0)
        {
            //Reload();
            return;
        }
        weaponObject.ammoAmount--;

        animator.SetInteger(State, 2);
        
        vfxController.Shoot();

        var transform1 = vfxController.transform;
        var bulletGO = Instantiate(bullet, transform1.position + transform1.forward, transform1.rotation);
        //bulletGO.GetComponent<Rigidbody>().velocity = transform.forward * weaponObject.BulletSpeed;
        var bulletComponent = bulletGO.GetComponent<BulletComponent>();
        bulletComponent.damageAmount = (int)weaponObject.damage;
        bulletComponent.owner = this.gameObject;
        
        vfxController.Shoot();
        ammo.text = weaponObject.ammoAmount.ToString() + " / " + weaponObject.maxAmmoAmount;
    }

    void Reload()
    {
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
        
        ammo.text =  weaponObject.ammoAmount.ToString() + " / " + weaponObject.maxAmmoAmount;
    }
}
