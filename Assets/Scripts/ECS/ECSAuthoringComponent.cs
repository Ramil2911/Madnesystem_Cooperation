using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WIP
[RequireComponent(typeof(ToECS))]
public class ECSAuthoringComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public unsafe struct ECSComponent
{
    public ulong id;
}