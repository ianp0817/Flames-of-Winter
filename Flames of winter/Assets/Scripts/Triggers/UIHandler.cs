using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] CanvasRenderer UIElement;
    [SerializeField] List<string> allowedTags;
    [SerializeField] bool requireCannon;
    [SerializeField] float displayDuration = 5f;
    [SerializeField] float fadeDuration = 0.5f;

    private bool canTrigger = true;

    private void Start()
    {
        UIElement.SetAlpha(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTrigger)
        {
            foreach (string tag in allowedTags)
            {
                if (other.CompareTag(tag))
                {
                    SolaraShoot solaraShoot = other.GetComponentInChildren<SolaraShoot>();
                    BobShoot bobShoot = other.GetComponentInChildren<BobShoot>();

                    if (!requireCannon || (solaraShoot ? solaraShoot.hasCannon : (bobShoot ? bobShoot.hasCannon : false)))
                    {
                        canTrigger = false;
                        StartCoroutine(DisplayUI());
                        return;
                    }
                }
            }
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
        Destroy(gameObject);
    }
}
