using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject parent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != parent)
            Destroy(gameObject);
    }
}
