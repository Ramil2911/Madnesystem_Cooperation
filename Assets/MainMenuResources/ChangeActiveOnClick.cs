using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveOnClick : MonoBehaviour
{
    public GameObject changeObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeActive()
    {
        if(changeObject.activeSelf == true)
        {
            changeObject.SetActive(false);
        }
        else
        {
            changeObject.SetActive(true);
        }
    }
}
