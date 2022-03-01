using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using System.Linq;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemyPercent = 50;
    [SerializeField] private int activeAttackers = 1;
    public MusicManager musicManager;

    private bool _wasAlreadyIn = false;
    private BoxCollider box;
    private bool isISwitchMusic = false;
    private List<Transform> enemiesPositions = new List<Transform>();
    private bool shot = false;

    [SerializeField] private List<EnemyPathfinding> holdingEnemies = new List<EnemyPathfinding>();
    [SerializeField] private List<EnemyPathfinding> attackingEnemies = new List<EnemyPathfinding>();


    private void Start()
    {
        foreach (Transform child in transform.Find("EnemyPositions"))
        {
            enemiesPositions.Add(child);
        }
        box = GetComponent<BoxCollider>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }
    void FixedUpdate()
    {
        if (_wasAlreadyIn)
        {
            CheckForFightEnd();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnableRendering();
            if (!_wasAlreadyIn)
            {
                CloseDoors();
                SpawnEnemies();
                _wasAlreadyIn = true;
                Destroy(box);
            }
        } //хз вообще почему occlusion culling не работает
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableRendering();
        }
    }

    private void CloseDoors()
    {
        Door[] doors = transform.Find("Doors").GetComponentsInChildren<Door>();
        musicManager.isBattleMod = true;
        isISwitchMusic = true;
        foreach (Door door in doors)
        {
            door.Close();
            door.Lock();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPositions.Count; i++)
        {
            int roll = Random.Range(1, 101);
            if (roll <= enemyPercent)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                holdingEnemies.Add(Instantiate(enemyPrefabs[enemyIndex], enemiesPositions[i].position + new Vector3(0, 3f, 0), Quaternion.identity, transform.Find("Enemies").transform).GetComponent<EnemyPathfinding>());
            }
        }
        StartCoroutine("ChangeHoldingPositions");
        AssignAttackers();

        CheckForFightEnd();
    }

    public void AssignAttackers()
    {
        while (holdingEnemies.Count != 0 && attackingEnemies.Count < activeAttackers) {
            int roll = Random.Range(0, holdingEnemies.Count);
            print(holdingEnemies.Count.ToString() + " " + roll.ToString());
            attackingEnemies.Add(holdingEnemies[roll]);
            holdingEnemies.RemoveAt(roll);
            EnemyPathfinding currentEnemy = attackingEnemies.Last();
            currentEnemy.isAttacker = true;
            if (currentEnemy.holdPosition != null) {
                currentEnemy.holdPosition.GetComponent<PositionManager>().FreePosition();
                currentEnemy.holdPosition = null;
            }
            
        }

        AssignHoldingPositions();
    }

    private void AssignHoldingPositions() {
        holdingEnemies = holdingEnemies.Where(item => item != null).ToList();
        for(int i = 0; i < holdingEnemies.Count; i++)
        {
            EnemyPathfinding enemy = holdingEnemies[i];
            float maxLengthFromPlayer = -1f;
            int selectedPosition = 0;
            List<Transform> unusedEnemiesPositions = enemiesPositions.FindAll(e => !e.GetComponent<PositionManager>().GetIsTaked());
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            for (int j = 0; j < enemiesPositions.Count; j++)
            {
                float distance = Vector3.Distance(enemiesPositions[j].position, playerPosition);
                if (distance > maxLengthFromPlayer)
                {
                    maxLengthFromPlayer = distance;
                    selectedPosition = j;
                }
            }
            enemy.holdPosition = enemiesPositions[selectedPosition];
            enemy.holdPosition.GetComponent<PositionManager>().TakePosition();
            enemy.isAttacker = false;
        }
    }


    IEnumerator ChangeHoldingPositions () {
        while (true) {
            AssignHoldingPositions();
            yield return new WaitForSeconds(1f);
        }
    }


    public void CheckForFightEnd()
    {
        
        if (transform.Find("Enemies").childCount <= 0)
        {
            if (isISwitchMusic == true)
            {
                musicManager.isBattleMod = false;
                isISwitchMusic = false;
            }
            FightEndSequence();
        }
    }

    private void FightEndSequence()
    {
        shot = true;
        StopCoroutine(nameof(ChangeHoldingPositions));
        OpenDoors();
    }

    private void OpenDoors()
    {
        Door[] doors = transform.Find("Doors").GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            door.Open();
            door.Unlock();
        }

    }
    public void ClearDeadEnemy(EnemyPathfinding enemy) {
        attackingEnemies.Remove(enemy);
    }

    
    private void EnableRendering()
    {
        if (transform.parent.TryGetComponent<MeshRenderer>(out var renderer))
        {
            renderer.enabled = true;
        }
        var child = transform.GetChild(3);
        if (child != null)
        {
            child.gameObject.SetActive(true);
        }
        /*foreach (var comp in GetComponentsInChildren<MeshRenderer>())
        {
            comp.enabled = true;
        }*/
    }
    
    private void DisableRendering()
    {
        if (transform.parent.TryGetComponent<MeshRenderer>(out var renderer))
        {
            renderer.enabled = false;
        }

        var child = transform.GetChild(3);
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
        /*foreach (var comp in GetComponentsInChildren<MeshRenderer>())
        {
            comp.enabled = false;
        }*/
    }
}
