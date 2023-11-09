using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptor : MonoBehaviour
{
    [SerializeField] GameObject unpowered;
    [SerializeField] GameObject powered;
    [SerializeField] List<Powerable> targets;
    private int sources = 0;

    private void AddSource()
    {
        if (sources++ == 0)
        {
            foreach (Powerable target in targets)
                target.IncreasePower();

            powered.SetActive(true);
            unpowered.SetActive(false);
        }
    }

    private void RemoveSource()
    {
        if (--sources == 0)
        {
            foreach (Powerable target in targets)
                target.DecreasePower();

            unpowered.SetActive(true);
            powered.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Energy"))
            AddSource();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Energy"))
            RemoveSource();
    }
}
