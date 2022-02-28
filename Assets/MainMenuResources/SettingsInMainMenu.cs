using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SettingsInMainMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdownQuality;

    public Volume curPostProcessing;
    public VolumeProfile[] allPostProcessings;

    public TMPro.TMP_InputField mouseSensitivity;

    public Slider musicVolumeSlider;
    public Slider soundsVolumeSlider;

    public SettingsApplier applier;
    // Start is called before the first frame update
    void Start()
    {
        applier ??= GameObject.Find("Player").GetComponent<SettingsApplier>();
        SetStartVolumes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStartVolumes()
    {
        if(PlayerPrefs.HasKey("idQuality"))
        {
            dropdownQuality.value = PlayerPrefs.GetInt("idQuality");
            CheckDropDown();
        }
        if(PlayerPrefs.HasKey("mouseSensitivity"))
        {
            mouseSensitivity.text = PlayerPrefs.GetFloat("mouseSensitivity").ToString();
        }
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if(PlayerPrefs.HasKey("SoundVolume"))
        {
            soundsVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
        gameObject.SetActive(false);
        applier.ApplySensitivity();
        applier.ApplyMusicSound();
        applier.ApplyEnvironmentSound();
    }
    public void CheckDropDown()
    {
        QualitySettings.SetQualityLevel(dropdownQuality.value, true); //Изменяем уровен графики

        PlayerPrefs.SetInt("idQuality", dropdownQuality.value);
        switch(dropdownQuality.value)
        {
            case 0:
                curPostProcessing.profile = allPostProcessings[0];
            break;
            case 1:
                curPostProcessing.profile = allPostProcessings[1];
            break;
            case 2:
                curPostProcessing.profile = allPostProcessings[2];
            break;
            case 3:
                curPostProcessing.profile = allPostProcessings[3];
            break;
            case 4:
                curPostProcessing.profile = allPostProcessings[4];
            break;
            case 5:
                curPostProcessing.profile = allPostProcessings[5];
            break;
        }

        
    }

    public void SetMouseSensitivity()
    {
        int curMouseSensitivity = 1;
        if(PlayerPrefs.HasKey("mouseSensitivity"))
        {
            curMouseSensitivity = PlayerPrefs.GetInt("mouseSensitivity");
        }
        try
        {
            curMouseSensitivity = int.Parse(mouseSensitivity.text);
            print(curMouseSensitivity);
        }
        catch
        {
            //владик лох
        }
        PlayerPrefs.SetInt("mouseSensitivity", curMouseSensitivity);
        applier.ApplySensitivity();
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        applier.ApplyMusicSound();
    }
    public void ChangeSoundVolume()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundsVolumeSlider.value);
        applier.ApplyEnvironmentSound();
    }
}