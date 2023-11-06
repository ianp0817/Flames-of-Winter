using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;

    public void TransitionIn(System.Action callback = null)
    {
        StartCoroutine(CoroutineIn(callback));
    }

    public void TransitionOut(System.Action callback = null)
    {
        StartCoroutine(CoroutineOut(callback));
    }

    private IEnumerator CoroutineIn(System.Action callback)
    {
        RectTransform rect = GetComponent<RectTransform>();
        float height = rect.rect.height;
        rect.anchoredPosition = new Vector2(0, 0);

        for (float y = 0f; y >= -height; y -= speed * height * Time.deltaTime )
        {
            rect.anchoredPosition = new Vector2(0, y);
            yield return null;
        }

        rect.anchoredPosition = new Vector2(0, -height);
        callback?.Invoke();
    }

    private IEnumerator CoroutineOut(System.Action callback)
    {
        RectTransform rect = GetComponent<RectTransform>();
        float height = rect.rect.height;
        rect.anchoredPosition = new Vector2(0, height);

        for (float y = height; y >= 0; y -= speed * height * Time.deltaTime)
        {
            rect.anchoredPosition = new Vector2(0, y);
            yield return null;
        }

        rect.anchoredPosition = new Vector2(0, 0);
        callback?.Invoke();
    }
}
