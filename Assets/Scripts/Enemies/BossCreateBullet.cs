using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCreateBullet : MonoBehaviour
{
    [SerializeField] private BossPathfinding _enemyPathfinding;
    [SerializeField] private GameObject bulletPrefab;
    //[SerializeField] private float _bulletSpeed = 10f;
    private Transform _player;
    public float delayShoot = 3f;

    private List<GameObject> _bulletList = new List<GameObject>();

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(FireEverySecond());
    }
    IEnumerator FireEverySecond()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (_enemyPathfinding.canAttack)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                _bulletList.Add(bullet);
                Transform target = _player.Find("TargetBullet").transform;
                bullet.transform.LookAt(target);
                bullet.GetComponent<Rigidbody>().velocity = transform.forward;
                bullet.GetComponent<BulletComponent>().owner = this.gameObject;
            }
            yield return new WaitForSeconds(delayShoot);
        }
    }
}
