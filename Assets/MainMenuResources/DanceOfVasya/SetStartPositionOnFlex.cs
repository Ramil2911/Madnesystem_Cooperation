using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartPositionOnFlex : MonoBehaviour
{
    private Vector3 startPos;
    private float timer;
    private float timerForSetPos = 104f;
    
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if(timer >= timerForSetPos)
        {
            
            transform.position = startPos;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
