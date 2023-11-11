using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] IceController iceController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BobProjectile"))
        {
            iceController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bob"))
            FindObjectOfType<BobShoot>().Block();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bob"))
            FindObjectOfType<BobShoot>().Unblock();
    }
}
