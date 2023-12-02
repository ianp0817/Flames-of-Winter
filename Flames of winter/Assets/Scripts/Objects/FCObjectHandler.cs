using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCObjectHandler : MonoBehaviour
{
    [SerializeField]
    private int coreIndex = 0;

    private void Start()
    {
        try
        {
            Persistent.FCBits &= ~(1 << coreIndex);
        } catch (System.Exception) { }
    }

    public void Collect()
    {
        try
        {
            Persistent.FCBits |= (1 << coreIndex);
        }
        catch (System.Exception) { }
        Destroy(gameObject);
    }
}
