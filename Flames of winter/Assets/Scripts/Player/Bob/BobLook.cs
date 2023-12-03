using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobLook : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.fixedDeltaTime) * Persistent.SensitivityY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.fixedDeltaTime) * Persistent.SensitivityX);
    }
}
