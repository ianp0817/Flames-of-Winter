using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTransition : MonoBehaviour
{
    [SerializeField] string targetScene;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += Transition;
    }

    private void Transition(VideoPlayer vp)
    {
        SceneManager.LoadScene(targetScene);
    }
}
