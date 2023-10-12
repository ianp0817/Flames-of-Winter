using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolaraSwap : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public SwapResult Swap(GameObject bob)
    {
        if (bob == null)
            return SwapResult.None;

        if (!Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, ~(1<<2)))
            return SwapResult.Follow;

        if (bob.Equals(hit.collider.gameObject))
            return SwapResult.Swap;

        return SwapResult.Follow;
    }
}
