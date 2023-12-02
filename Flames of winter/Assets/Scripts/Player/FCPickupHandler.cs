using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCPickupHandler : MonoBehaviour
{
    public void Pickup()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3))
            if (hit.transform.CompareTag("FusionCore"))
                hit.transform.GetComponent<FCObjectHandler>().Collect();
    }
}
