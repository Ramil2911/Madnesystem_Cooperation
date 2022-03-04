using UnityEngine;
using UnityEngine.UI;
using UniversalMobileController;

public abstract class WeaponController : MonoBehaviour
{
    public bool isActive = true;
    
    public RecoilController recoilController;
    public WeaponDescription weaponObject;
    public Text ammo;
    public Animator animator;
    public GameObject bullet;
    
    public Vector2 impact = new(.1f, .1f);
    public float impactDuration = 0.1f;
    
    private SpecialButton button;
    protected Text ammoText;
    public VFXController vfxController;
    
    public bool doIShoot;
    public bool amIReloading;
    public bool shootWasOneFrameBefore;
    
    protected static readonly int State = Animator.StringToHash("State");

    protected void FindUI()
    {
        button = GameObject.FindWithTag("ShootButton").GetComponent<SpecialButton>();
        ammoText = GameObject.FindWithTag("AmmoText").GetComponent<Text>();
    }

    protected void RunCommonShootingStuff()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (doIShoot == false)
                animator.SetInteger(State, 1);
        }
        else
        {
            if (doIShoot == false)
                animator.SetInteger(State, 0);
        }

        if (weaponObject.weaponType == WeaponType.Auto && button.isPressed)
        {
            Shoot();
        }
        else if (weaponObject.weaponType == WeaponType.SemiAuto && button.isDown)
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

    public abstract void Shoot();

    public abstract void Reload();
}
