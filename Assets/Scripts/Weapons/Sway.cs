using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    private Quaternion oldRotation;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        oldRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var transform1 = transform;
        transform1.rotation = Quaternion.identity;
        transform.rotation = Quaternion.Lerp(oldRotation, transform1.parent.rotation, speed);
        oldRotation = transform1.rotation;
    }
}
