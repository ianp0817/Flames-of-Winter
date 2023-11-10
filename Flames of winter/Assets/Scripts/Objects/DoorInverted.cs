using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorInverted : Powerable
{
    [SerializeField] GameObject panel;

    protected override void OnNotPowered()
    {
        panel.SetActive(false);
    }

    protected override void OnPowered()
    {
        panel.SetActive(true);
    }

    protected override void WhileNotPowered()
    {
        // Nothing
    }

    protected override void WhilePowered()
    {
        // Nothing
    }
}
