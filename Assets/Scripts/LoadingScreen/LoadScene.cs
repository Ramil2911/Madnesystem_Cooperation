using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    private bool isLoading;
    private AsyncOperation operation;

    private Slider slider;
    private int id;
    
    public GameObject ui;
    
    

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Load(int index)
    {
        id = index;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (!isLoading)
        {
            slider = GameObject.FindWithTag("ProgressBar").GetComponent<Slider>();
            operation = SceneManager.LoadSceneAsync(id);
            isLoading = true;
            return;
        }
        slider.value = operation.progress;

        if (operation.isDone)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            Destroy(this.gameObject);
        }
    }
}
