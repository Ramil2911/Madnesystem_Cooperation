using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using UnityEngine;

public class SetMouseSensitivity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
        GetComponent<FPSController>().sensitivity = sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
