using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using UnityEngine;

public class SettingsApplier : MonoBehaviour
{
    [SerializeField]
    private FPSController _controller;
    public FPSController controller
    {
        get
        {
            _controller ??= GetComponent<FPSController>();
            return _controller;
        }
    }
    [SerializeField]
    private Camera _camera;
    public new Camera camera
    {
        get
        {
            _camera ??= GetComponent<Camera>();
            return _camera;
        }
    }
    
    void Start()
    {
        ApplyMusicSound();
        ApplyEnvironmentSound();
        ApplySensitivity();
    }

    public void ApplyMusicSound()
    {
        
    }
    
    public void ApplyEnvironmentSound()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("SoundVolume");
    }

    public void ApplySensitivity()
    {
        controller.sensitivity = PlayerPrefs.GetInt("mouseSensitivity");
    }
}
