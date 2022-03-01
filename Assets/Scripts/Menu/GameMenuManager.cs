using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameMenuManager : MonoBehaviour
{
    public GameObject gameMenu;

    public GameObject settingsPanel;

    public static Volume curPostProcessing;

    public VolumeProfile[] allPostProcessings;

    public GameObject player;
    public PlayerDisabler disabler;
    private static readonly string idqualityKey = "idQuality";

    void Start()
    {
       ChangeQuality();
    }
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeActiveGameMenu();
        }
    }
    public void ChangeActiveGameMenu()
    {
        if(gameMenu.activeSelf == false)
        {
            Time.timeScale = 0;
            gameMenu.SetActive(true);
            disabler.Disable();
        }
        else
        {
            Time.timeScale = 1;
            gameMenu.SetActive(false);
            disabler.Enable();
            if(settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
            }
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReturnToGame() //на ui
    {
        Cursor.visible = false;
        gameMenu.SetActive(false);
        Time.timeScale = 1;  
        if(settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void ChangeActiveSettingsPanel()
    {
        if(!settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
        }
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
