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

    [SerializeField]private List<EnemyPathfinding> holdingEnemies = new List<EnemyPathfinding>();
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
        if (!_wasAlreadyIn)
        {
            if (other.CompareTag("Player"))
            {
                CloseDoors();
                SpawnEnemies();
                _wasAlreadyIn = true;
                Destroy(box);
            }
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

        AssignAttackers();

        CheckForFightEnd();
    }

    public void AssignAttackers()
    {
        while (attackingEnemies.Count < activeAttackers) {
            int roll = Random.Range(0, holdingEnemies.Count);
            attackingEnemies.Add(holdingEnemies[roll]);
            holdingEnemies.RemoveAt(roll);
            attackingEnemies[roll].isAttacker = true;
        }

        foreach (EnemyPathfinding enemy in holdingEnemies) {
            float maxLengthFromPlayer = -1f;
            int selectedPosition = 0;
            List<Transform> unusedEnemiesPositions = enemiesPositions.FindAll(e => !e.GetComponent<PositionManager>().GetIsTaked());
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            for (int i = 0; i < enemiesPositions.Count; i++) {
                float distance = Vector3.Distance(enemiesPositions[i].position, playerPosition);
                if (distance > maxLengthFromPlayer) {
                    maxLengthFromPlayer = distance;
                    selectedPosition = i;
                }
            }
            enemy.holdPosition = enemiesPositions[selectedPosition];
            enemy.isAttacker = false;
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
}
