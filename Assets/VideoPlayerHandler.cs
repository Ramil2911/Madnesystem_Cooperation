using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerHandler : MonoBehaviour
{
    public GameObject loader;
    public VideoPlayer vid;


    void Start() { vid.loopPointReached += CheckOver; }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("BossFight");
    }

}
