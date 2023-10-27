using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SolaraButton : Interactable
{
    [SerializeField] ParentConstraint button;
    [SerializeField] float moveThreshold = 0.1f;
    [SerializeField] List<Powerable> targets;
    private Vector3 defaultPos;
    private GameObject interactor;
    private Vector3 interactPosition;

    public override bool Interact(GameObject interactor)
    {
        if (!this.interactor)
        {
            Press(interactor);
            return true;
        }
        else
        {
            Release();
            return false;
        }
    }

    private void Press(GameObject interactor)
    {
        this.interactor = interactor;
        interactPosition = interactor.transform.position;
        foreach (Powerable target in targets)
            target.SetPower(true);

        button.SetTranslationOffset(0, Vector3.zero);
    }

    private void Release()
    {
        interactor = null;
        foreach (Powerable target in targets)
            target.SetPower(false);

        button.SetTranslationOffset(0, defaultPos);
    }

    private void Start()
    {
        defaultPos = button.GetTranslationOffset(0);
    }

    private void Update()
    {
        if (interactor && Vector3.Distance(interactor.transform.position, interactPosition) > moveThreshold)
            Release();
    }
}
