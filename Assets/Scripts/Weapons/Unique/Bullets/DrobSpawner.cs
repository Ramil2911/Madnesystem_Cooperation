using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrobSpawner : MonoBehaviour
{
    public float x1 = -1;
    public float x2 = 1;
    public float y1 = -1;
    public float y2 = 1;

    public float damage;

    public int bulletAmount = 7;

    public GameObject bullet;

    public GameObject owner;
    private void Start()
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            var bulletGO = Instantiate(bullet,
                transform.position + transform.right * Random.Range(x1,x2) + transform.up*Random.Range(y1,y2),
                Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            var bulletComponent = bulletGO.GetComponent<BulletComponent>();
            bulletComponent.damageAmount = damage*WeaponBuff.damageMultiplier;
            bulletComponent.owner = owner;
        }
    }
}
