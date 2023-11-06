using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    [SerializeField] private string targetScene;

    private TransitionHandler transitionHandler;
    private GameObject solara;
    private GameObject bob;

    private bool needsSolara;
    private bool needsBob;
    private bool containsSolara = false;
    private bool containsBob = false;

    void Start()
    {
        transitionHandler = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TransitionHandler>();
        solara = GameObject.FindGameObjectWithTag("Solara");
        bob = GameObject.FindGameObjectWithTag("Bob");

        needsSolara = solara != null;
        needsBob = bob != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == solara)
        {
            containsSolara = true;
            tryChange();
        }
        if (other.gameObject == bob)
        {
            containsBob = true;
            tryChange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == solara)
            containsSolara = false;
        if (other.gameObject == bob)
            containsBob = false;
    }

    private void tryChange()
    {
        if ((!needsSolara || containsSolara) && (!needsBob || containsBob))
        {
            transitionHandler.TransitionOut(() =>
                SceneManager.LoadScene(targetScene)
            );
        }
    }
}
