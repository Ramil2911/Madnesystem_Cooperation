using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerHandler : MonoBehaviour
{
    public GameObject loader;

    void Start() {
        GameObject camera = GameObject.Find("Main Camera");
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp) {
        // Instantiate(loader).GetComponent<LoadScene>().Load(4);
        print("BOSSSSSS FIGHT");
    }
}
