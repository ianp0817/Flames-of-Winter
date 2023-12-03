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

                    break;
                case ButtonType.MenuSettings:

                    break;
                case ButtonType.TitleScreen:
                    SceneManager.LoadScene(0);
                    break;
                case ButtonType.Back:

                    break;
            }
        }
    }
}
