using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEndShoot : MonoBehaviour
{
    RevolverController revolverController;
    // Start is called before the first frame update
    void Start()
    {
        revolverController = GetComponentInParent<RevolverController>();
    }
    public void IShoot()
    {
        revolverController.doIShoot = true;
    }
    public void EndShoot()
    {
        revolverController.doIShoot = false;
    }
}
