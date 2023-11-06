using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] IceController iceController;
    private BobShoot bobShoot;

    private void Awake()
    {
        bobShoot = FindObjectOfType<BobShoot>();
    }

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
            bobShoot.Block();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bob"))
            bobShoot.Unblock();
    }
}
