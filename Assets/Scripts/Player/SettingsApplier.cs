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
        if (controller == null)
        {
            Debug.Log("no FPSController set");
        }
        else
        {
            controller.sensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
        }
    }
}
