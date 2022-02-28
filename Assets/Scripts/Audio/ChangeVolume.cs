using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolume : MonoBehaviour
{
    public bool isMusic = false;
    private AudioSource audioSource;
    
    private float startVolume;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
        ChangeThisVolume();
    }
    public void ChangeThisVolume()
    {
        if(isMusic == true)
        {
            if(PlayerPrefs.HasKey("MusicVolume"))
            {
                audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            }
            else
            {
                audioSource.volume = startVolume;
            }
            
        }   
        else
        {
            if(PlayerPrefs.HasKey("SoundVolume"))
            {
                audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
            }
            else
            {
                audioSource.volume = startVolume;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
