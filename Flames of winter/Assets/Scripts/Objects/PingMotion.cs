using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingMotion : MonoBehaviour
{
    [SerializeField] float amplitude = 0.25f;
    [SerializeField] float cyclesPerSecond = 1f;
    private float initialY;

    void Start()
    {
        initialY = transform.localPosition.y;
    }

    void Update()
    {
        transform.localPosition = new(0, initialY + amplitude * Mathf.Sin(Time.time * cyclesPerSecond * 2 * Mathf.PI), 0);
    }
}
