using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SettingsInMainMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdownQuality;

    public Volume curPostProcessing;
    public VolumeProfile[] allPostProcessings;

    public Slider mouseSensitivity;
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
            mouseSensitivity.value = PlayerPrefs.GetFloat("mouseSensitivity");
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
        curPostProcessing.profile = allPostProcessings[dropdownQuality.value];
    }

    public void SetMouseSensitivity()
    {
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity.value);
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