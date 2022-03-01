using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EntityComponent : MonoBehaviour
{
    //когда я это писал, в моей голове еще были мысли о коопе, далее такого уже нет, лишь бы написать
    public float Health = 100;
    public float MaxHealth = 100;
    public float Regeneration = 0.2f;
    
    public bool isImmuneToFire;
    public bool isImmuneToRadiation;

    public UnityEvent<float> HealthChangedEvent = new UnityEvent<float>();
    public UnityEvent EntityDiedEvent = new UnityEvent();
    
    public GameObject explosion;

    public bool isPlayer = false;
    private void BroadcastMessageIfDead()
    {
        //EntityDiedEvent.Invoke();
    }

    public void Damage(float amount, EntityComponent source)
    {
        Health -= amount;
        HealthChangedEvent.Invoke(Health);
        if (Health <= 0)
        {
            //BroadcastMessageIfDead();
            Dead();
        }

        //это выглядит очень странно, но мне не приходит в голову, как бы это можно было бы усложнить/улучшить.
    }

    public void Dead()
    {

        if(isPlayer == false)
        {
            EnemyDead();
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
        }
        else if(isPlayer == true)
        {
            SceneManager.LoadScene(1);
        }
        Destroy(gameObject);
    }

    private void EnemyDead()
    {
        if (transform.TryGetComponent<EnemyPathfinding>(out var enemy))
        {
            if (enemy.isAttacker) {
                FightManager fightManager = transform.parent.parent.GetComponent<FightManager>();
                fightManager.ClearDeadEnemy(enemy);
                fightManager.AssignAttackers();
            }
            if (enemy.holdPosition)
            {
                enemy.holdPosition.GetComponent<PositionManager>().FreePosition();
                enemy.holdPosition = null;
            }
        }
    }
}


