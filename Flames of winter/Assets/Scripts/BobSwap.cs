using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobSwap : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public SwapResult Swap(GameObject solara)
    {
        if (solara == null)
            return SwapResult.None;

        if (!Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, ~(1 << 2)))
            return SwapResult.None;

        if (hit.collider.gameObject != null && solara.Equals(hit.collider.gameObject))
            return SwapResult.Swap;

        return SwapResult.None;
    }
}
