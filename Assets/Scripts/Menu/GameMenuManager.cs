using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UniversalMobileController;

public class GameMenuManager : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject settingsPanel;
    public GameObject abilitiesMenu;

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
        if(abilitiesMenu.activeSelf) return;
        if (gameMenu.activeSelf)
        {
            ReturnToGame();
        }
        else
        {
            for (int i = 0; i < onScreenControls.Length; i++)
            {
                onScreenControls[i].SetActive(false);
            }

            Time.timeScale = 0;
            gameMenu.SetActive(true);
            disabler.Disable();
        }
    }
    
    public void ChangeActiveSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    
    public void SwitchAbilitiesPanel()
    {
        if(gameMenu.activeSelf) return;
        if(abilitiesMenu.activeSelf)
        {
            ReturnToGame();
        }
        else
        {
            for (int i = 0; i < onScreenControls.Length; i++)
            {
                onScreenControls[i].SetActive(false);
            }
            Time.timeScale = 0;
            abilitiesMenu.SetActive(true);
            disabler.Disable();
        }
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReturnToGame() //на ui
    {
        //enable ingame controls
        for (int i = 0; i < onScreenControls.Length; i++)
        {
            onScreenControls[i].SetActive(true);
        }
        
        //
        Time.timeScale = 1;
        
        //enable everything related to player
        disabler.Enable();
        
        //disable all menus
        gameMenu.SetActive(false);
        settingsPanel.SetActive(false);
        abilitiesMenu.SetActive(false);
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
