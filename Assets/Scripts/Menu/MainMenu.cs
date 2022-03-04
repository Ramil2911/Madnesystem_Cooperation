using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loader;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    public void LoadScene(int idScene)
    {
        Instantiate(loader).GetComponent<LoadScene>().Load(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
