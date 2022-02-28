using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFromCameraToFlex : MonoBehaviour
{
    public Transform thisTransform;
    public Transform target;
    public Vector3 offsetPosition;
    public Vector3 Rotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //thisTransform.position = new Vector3(target.position.x + offsetPosition.x, target.position.y + offsetPosition.y, target.position.z + offsetPosition.z);

        thisTransform.position = new Vector3(target.position.x + offsetPosition.x, offsetPosition.y, target.position.z + offsetPosition.z);

        transform.Rotate(Rotate * Time.deltaTime);
    }
}
