using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTriggerHandler : MonoBehaviour
{
    [SerializeField]
    private int coreFragments;
    private const string secretEnding = "SecretEnding";
    private const string sacrificeSolara = "SacrificeSolara";
    private const string sacrificeBob = "SacrificeBob";

    private TransitionHandler transitionHandler;

    void Start()
    {
        transitionHandler = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TransitionHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasAllFragments())
        {
            transitionHandler.TransitionOut(() =>
                SceneManager.LoadScene(secretEnding)
            );
        }
        else if (other.CompareTag("Solara"))
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

    private bool hasAllFragments()
    {
        int fragmentBitField = Persistent.FCBits;
        int fragments = 0;

        while (fragmentBitField != 0)
        {
            fragments += fragmentBitField & 1;
            fragmentBitField >>= 1;
        }

        return fragments >= coreFragments;
    }
}
