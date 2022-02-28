using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameMenuManager : MonoBehaviour
{
    public GameObject gameMenu;

    public GameObject settingsPanel;

    public static Volume curPostProcessing;

    public VolumeProfile[] allPostProcessings;
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
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 1;
            gameMenu.SetActive(false);
            Cursor.visible = true;
            if(settingsPanel.activeSelf == true)
            {
                settingsPanel.SetActive(false);
            }
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReturnToGame()
    {
        Cursor.visible = false;
        gameMenu.SetActive(false);
        Time.timeScale = 1;  
        if(settingsPanel.activeSelf == true)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void ChangeActiveSettingsPanel()
    {
        if(settingsPanel.activeSelf == false)
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
        
        if(PlayerPrefs.HasKey("idPQuality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("idQuality"), true); //Изменяем уровен графики

            switch(PlayerPrefs.GetInt("idQuality"))
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
    }
}
