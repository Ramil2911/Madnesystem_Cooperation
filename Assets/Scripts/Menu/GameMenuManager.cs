using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UniversalMobileController;

public class GameMenuManager : MonoBehaviour
{
    public GameObject gameMenu;

    public GameObject settingsPanel;

    public Volume curPostProcessing;

    public VolumeProfile[] allPostProcessings;

    public GameObject player;
    public PlayerDisabler disabler;
    private static readonly string idqualityKey = "idQuality";

    public GameObject[] onScreenControls;

    public SpecialButton pauseButton;

    void Start()
    {
       ChangeQuality();
    }
    void Update() 
    {
        if(pauseButton.isDown)
        {
            ChangeActiveGameMenu();
        }
    }
    public void ChangeActiveGameMenu()
    {
        if(gameMenu.activeSelf == false)
        {
            for (int i = 0; i < onScreenControls.Length; i++)
            {
                onScreenControls[i].SetActive(false);
            }
            Time.timeScale = 0;
            gameMenu.SetActive(true);
            disabler.Disable();
        }
        else
        {
            ReturnToGame();
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReturnToGame() //на ui
    {
        for (int i = 0; i < onScreenControls.Length; i++)
        {
            onScreenControls[i].SetActive(true);
        }
        Time.timeScale = 1;
        gameMenu.SetActive(false);
        disabler.Enable();
        if(settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void ChangeActiveSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    public void ChangeQuality()
    {
        if(PlayerPrefs.HasKey(idqualityKey))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(idqualityKey), true); //Изменяем уровен графики
            curPostProcessing.profile = allPostProcessings[PlayerPrefs.GetInt(idqualityKey)];
        }
    }
}
