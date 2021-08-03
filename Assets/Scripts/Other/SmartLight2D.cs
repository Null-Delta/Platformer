using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using System.Reflection;
using UnityEngine;

public class SmartLight2D : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D shapeLight;
    Vector3[] points;
    void Start()
    {
        points = new Vector3[5];
        points[0] = new Vector3(1f, 1f, 0);
        points[1] = new Vector3(-1f, 1f, 0);
        points[2] = new Vector3(-1f, -1f, 0);
        points[3] = new Vector3(1f, -1f, 0);
        points[4] = new Vector3(1.5f, 0f, 0);

        shapeLight = gameObject.GetComponent<Light2D>();
        foreach(Vector3 v in shapeLight.shapePath)
            Debug.Log(v);

        SetShapePath(shapeLight, points);
    }

    void SetShapePath(Light2D light, Vector3[] path)
    {
        SetFieldValue<Vector3[]>(light, "m_ShapePath", path);
    }

    void SetFieldValue<T>(object obj, string name, T val)
    {
        var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(obj, val);
    }
}
