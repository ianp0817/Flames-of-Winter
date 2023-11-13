using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTriggerHandler : MonoBehaviour
{
    private const string sacrificeSolara = "SacrificeSolara";
    private const string sacrificeBob = "SacrificeBob";

    private TransitionHandler transitionHandler;

    void Start()
    {
        transitionHandler = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TransitionHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Solara"))
        {
            transitionHandler.TransitionOut(() =>
                SceneManager.LoadScene(sacrificeSolara)
            );
        } else if (other.CompareTag("Bob"))
        {
            transitionHandler.TransitionOut(() =>
                SceneManager.LoadScene(sacrificeBob)
            );
        }
    }
}
