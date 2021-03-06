using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDecor : MonoBehaviour
{
    [SerializeField] private GameObject[] decorPrefabs;
    [SerializeField] private int chance = 100;
    
    private DungeonGenerator generator;
    private bool isCompleted;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.Find("Generator").GetComponent<DungeonGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCompleted && generator.dungeonState == DungeonState.Completed)
        {
            isCompleted = true;
            int roll = Random.Range(0, 100);
            if (roll <= chance) {
                int decorIndex = Random.Range(0, decorPrefabs.Length);
                GameObject goDecor = Instantiate(decorPrefabs[decorIndex], transform.position, transform.rotation,
                    transform) as GameObject;
                goDecor.name = decorPrefabs[decorIndex].name;
            }
        }
    }
}
