using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonClick : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private ButtonType button;
    private enum ButtonType { None, NewGame, Continue, TitleSettings, Quit, Resume, MenuSettings, TitleScreen, Back }

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject settingsMenu;

    private void Start()
    {
        if (button == ButtonType.Continue && Persistent.LvlIdx == 0)
        {
            Destroy(GetComponent<MouseHover>());
            GetComponent<Image>().color = new(0.25f, 0.25f, 0.25f);
            Destroy(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.hovered.Contains(gameObject))
        {
            switch (button)
            {
                case ButtonType.NewGame:
                    Persistent.ClearData();
                    SceneManager.LoadScene("Bootup");
                    break;
                case ButtonType.Continue:
                    SceneManager.LoadScene(Persistent.LvlIdx);
                    break;
                case ButtonType.TitleSettings:
                    SceneManager.LoadScene("SettingsMenu");
                    break;
                case ButtonType.Quit:
                    Application.Quit();
                    break;
                case ButtonType.Resume:
                    FindObjectOfType<InputManager>()?.Enable();
                    FindObjectOfType<CutsceneHandler>()?.Enable();
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Destroy(transform.parent.parent.gameObject);
                    break;
                case ButtonType.MenuSettings:
                    Destroy(transform.parent.parent.gameObject);
                    Instantiate(settingsMenu);
                    break;
                case ButtonType.TitleScreen:
                    FindObjectOfType<InputManager>()?.ClearPause();
                    FindObjectOfType<CutsceneHandler>()?.ClearPause();
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                    break;
                case ButtonType.Back:
                    Destroy(transform.parent.parent.gameObject);
                    Instantiate(pauseMenu);
                    break;
            }
        }
    }
}
