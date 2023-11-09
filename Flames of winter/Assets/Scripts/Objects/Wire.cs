using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : Powerable
{
    [SerializeField] GameObject pointPrefab;
    [SerializeField] GameObject linePrefab;
    [SerializeField] Material conductor;
    [SerializeField] Material energy;
    private int renderersSize = 0;
    private int renderersCap = 10;
    private Renderer[] renderers;

    private void Start()
    {
        renderers = new Renderer[renderersCap];

        int numChildren = transform.childCount;
        for (int i = 0; i < numChildren; i++)
            CreatePoint(transform.GetChild(i));
    }

    private void CreatePoint(Transform point)
    {
        int numChildren = point.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            Transform child = point.GetChild(i);
            CreatePoint(child);
            CreateLine(point, child);
        }
        Add(Instantiate(pointPrefab, point).transform.GetChild(0).GetComponent<Renderer>());
    }

    private void CreateLine(Transform source, Transform dest)
    {
        Vector3 diff = dest.position - source.position;
        GameObject line = Instantiate(linePrefab, source.position, Quaternion.FromToRotation(Vector3.up, diff), source);
        line.transform.localScale = new(line.transform.localScale.x, diff.magnitude, line.transform.localScale.z);
        Add(line.transform.GetChild(0).GetComponent<Renderer>());
    }

    private void Add(Renderer renderer)
    {
        if (renderersSize == renderersCap)
        {
            renderersCap *= 2;
            Renderer[] result = new Renderer[renderersCap];
            renderers.CopyTo(result, 0);
            renderers = result;
        }
        renderers[renderersSize++] = renderer;
    }

    protected override void OnPowered()
    {
        for (int i = 0; i < renderersSize; i++)
            renderers[i].material = energy;
    }

    protected override void OnNotPowered()
    {
        for (int i = 0; i < renderersSize; i++)
            renderers[i].material = conductor;
    }

    protected override void WhilePowered()
    {
        // Nothing
    }

    protected override void WhileNotPowered()
    {
        // Nothing
    }
}
