using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTransition : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.CutsceneActions cutscene;

    [SerializeField] string targetScene;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += Transition;

        playerInput = new PlayerInput();
        cutscene = playerInput.Cutscene;
        cutscene.Skip.performed += ctx => Transition(videoPlayer);
        cutscene.Enable();
    }

    private void Transition(VideoPlayer vp)
    {
        cutscene.Disable();
        SceneManager.LoadScene(targetScene);
    }
}
