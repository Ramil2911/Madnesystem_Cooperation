using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using UnityEngine;

public class StatisticsComponent : MonoBehaviour
{
    public GameObject player;
    public float distance;

    private Vector3 _prevPos;
        
    // Update is called once per frame
    void Update()
    {
        var pos = player.transform.position;
        var dist = (pos - _prevPos).magnitude;
    }
}
