using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRooms : MonoBehaviour
{
    public GameObject[] rooms;
    public float timer;

    public Image imageBackground;
    public float stepAlpha;
    public float updateAlpha = 0.02f;


    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeRoom());
        rooms[i].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float alpha = 0;
    IEnumerator ChangeRoom()
    {
        yield return new WaitForSeconds(timer);
        
        while(imageBackground.color.a < 1f)
        {
            
            alpha += stepAlpha;

            imageBackground.color = new Color(imageBackground.color.r, imageBackground.color.g, imageBackground.color.b, alpha);
            yield return new WaitForSeconds(updateAlpha);
        }
        
        rooms[i].SetActive(false);
        if(i + 1 >= rooms.Length)
        {
            i = 0;
        }
        else
        {
            i++;
        }
        rooms[i].SetActive(true);
        while(imageBackground.color.a >= 0f)
        {
            
            alpha -= stepAlpha;

            imageBackground.color = new Color(imageBackground.color.r, imageBackground.color.g, imageBackground.color.b, alpha);
            yield return new WaitForSeconds(updateAlpha);
        }
        
        StartCoroutine(ChangeRoom());
        yield break;
    }
}
    
