using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] IceController iceController;
    private bool containsBob = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!containsBob && collision.gameObject.CompareTag("BobProjectile"))
        {
            iceController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bob"))
            containsBob = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bob"))
            containsBob = false;
    }
}
