using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTex : MonoBehaviour
{
    [SerializeField] float radius = 1f;
    [SerializeField] float minRadius = 0.5f;
    [SerializeField] float maxRadius = 2f;
    [SerializeField] float minScale = 1f;
    [SerializeField] float maxScale = 2f;
    [SerializeField] float degreeRate = 36f;
    [SerializeField] float centerMarchAmplitude = 0.1f;
    [SerializeField] float radiusMarchAmplitude = 0.1f;
    [SerializeField] float angleMarchAmplitude = 0.1f;
    [SerializeField] float scaleMarchAmplitude = 0.1f;
    private float radianDegreeRate;
    private float radianAngleMarchAmplitude;
    private Vector2 center = Vector2.zero;
    private float angle;
    private Vector2 scale = 1.5f * Vector2.one;
    private Renderer matRenderer;

    private void Start()
    {
        angle = Random.Range(0, 2 * Mathf.PI);
        matRenderer = GetComponent<Renderer>();
        radianDegreeRate = degreeRate * Mathf.Deg2Rad;
        radianAngleMarchAmplitude = angleMarchAmplitude * Mathf.Deg2Rad;
    }

    private void Update() {
        radianDegreeRate += Time.deltaTime * radianAngleMarchAmplitude * Random.Range(-1, 1);
        center += Time.deltaTime * centerMarchAmplitude * Random.Range(-1, 1) * new Vector2(Random.value, Random.value).normalized;
        radius += Time.deltaTime * radiusMarchAmplitude * Random.Range(-1, 1);
        angle += Time.deltaTime * radianDegreeRate;
        scale += Time.deltaTime * scaleMarchAmplitude * new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        radius = Mathf.Clamp(radius, minRadius, maxRadius);
        scale = new Vector2(Mathf.Clamp(scale.x, minScale, maxScale), Mathf.Clamp(scale.y, minScale, maxScale));

        float OffsetX = center.x + radius * Mathf.Cos(angle);
        float OffsetY = center.y + radius * Mathf.Sin(angle);

        matRenderer.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        matRenderer.material.mainTextureScale = scale;
    }
}
