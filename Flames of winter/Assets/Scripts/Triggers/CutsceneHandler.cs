using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CutsceneHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.CutsceneActions cutscene;
    private PlayerInput.GlobalActions global;

    [SerializeField] CanvasRenderer UIElement;
    [SerializeField] float displayDuration = 5f;
    [SerializeField] float fadeDuration = 0.5f;
    private bool skipTooltip = false;

    [SerializeField] string targetScene;
    private VideoPlayer videoPlayer;

    [SerializeField] GameObject pauseMenu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += Transition;

        playerInput = new PlayerInput();
        cutscene = playerInput.Cutscene;
        cutscene.Skip.performed += ctx => Transition(videoPlayer);
        cutscene.Tooltip.performed += DisplayTooltip;
        cutscene.Enable();

        global = playerInput.Global;
        SceneManager.sceneUnloaded += (scene) => {
            ClearPause();
        };
        global.Pause.performed += Pause;
        global.Enable();

        UIElement.SetAlpha(0);
    }

    private void Transition(VideoPlayer vp)
    {
        cutscene.Disable();
        SceneManager.LoadScene(targetScene);
    }

    private void DisplayTooltip(InputAction.CallbackContext ctx)
    {
        if(!skipTooltip && !Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            StartCoroutine(DisplayUI());
            skipTooltip = true;
        }
    }

    private IEnumerator DisplayUI()
    {
        float startTime = Time.time;
        float alpha;

        while ((alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeDuration)) < 1)
        {
            UIElement.SetAlpha(alpha);
            yield return null;
        }

        UIElement.SetAlpha(1);
        startTime = Time.time;

        while (Time.time - startTime < displayDuration)
            yield return null;

        startTime = Time.time;

        while ((alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeDuration)) > 0)
        {
            UIElement.SetAlpha(alpha);
            yield return null;
        }

        UIElement.SetAlpha(0);
        skipTooltip = false;
    }

    public void Enable()
    {
        cutscene.Enable();
    }

    public void Disable()
    {
        cutscene.Disable();
    }

    private void Pause(InputAction.CallbackContext ctx)
    {
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        if (!eventSystem)
        {
            Disable();
            Time.timeScale = 0;
            Instantiate(pauseMenu);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Enable();
            Time.timeScale = 1;
            Destroy(FindObjectOfType<EventSystem>().transform.parent.gameObject);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ClearPause()
    {
        global.Pause.performed -= Pause;
    }
}
