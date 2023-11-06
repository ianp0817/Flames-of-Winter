using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static readonly float lifetime = 10f;
    public GameObject parent;

    private float timeRemaing = lifetime;

    private void Update()
    {
        if ((timeRemaing -= Time.deltaTime) <= 0f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != parent)
            Destroy(gameObject);
    }
}
