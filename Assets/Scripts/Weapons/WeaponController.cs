using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public bool isActive = true;
    public RecoilController recoilController;
    public WeaponDescription weaponObject;
    public Text ammo;
    public Animator animator;
    public bool doIShoot = false;
}
