using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AbilitiesUISwitcher : MonoBehaviour
{
    public GameObject panel;
    
    
    public void Switch()
    {
       panel.SetActive(!panel.activeSelf);
       
    }
}
