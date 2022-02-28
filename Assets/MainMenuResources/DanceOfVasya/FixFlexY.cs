using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixFlexY : MonoBehaviour
{

    public Transform thisPosition;
    public float setPositionY;

    
    void LateUpdate()
    {
        if(thisPosition.position.y < setPositionY || thisPosition.position.y > -setPositionY / 2f)
        {
            thisPosition.position = new Vector3(thisPosition.position.x, setPositionY, thisPosition.position.z);
        }
    }
}
