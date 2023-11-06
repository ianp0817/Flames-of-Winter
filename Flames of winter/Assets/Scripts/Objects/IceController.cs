using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController : MonoBehaviour
{
    [SerializeField] WaterController waterController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SolaraProjectile"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
