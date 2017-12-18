using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtils {
    // TODO make rotation work
    public static void DrawBox(Vector3 center, Vector3 half, Quaternion rotation, Color color)
    {
        Vector3 v0 = center - half;
        Vector3 v1 = center + half;
        float t = 120f;
        Debug.DrawLine(new Vector3(v0.x, v0.y, v0.z), new Vector3(v1.x, v0.y, v0.z), color, t);
        Debug.DrawLine(new Vector3(v0.x, v0.y, v0.z), new Vector3(v0.x, v1.y, v0.z), color, t);
        Debug.DrawLine(new Vector3(v0.x, v0.y, v0.z), new Vector3(v0.x, v0.y, v1.z), color, t);

        Debug.DrawLine(new Vector3(v1.x, v0.y, v0.z), new Vector3(v1.x, v1.y, v0.z), color, t);
        Debug.DrawLine(new Vector3(v1.x, v0.y, v0.z), new Vector3(v1.x, v0.y, v1.z), color, t);

        Debug.DrawLine(new Vector3(v0.x, v1.y, v0.z), new Vector3(v1.x, v1.y, v0.z), color, t);
        Debug.DrawLine(new Vector3(v0.x, v1.y, v0.z), new Vector3(v0.x, v1.y, v1.z), color, t);

        Debug.DrawLine(new Vector3(v0.x, v0.y, v1.z), new Vector3(v1.x, v0.y, v1.z), color, t);
        Debug.DrawLine(new Vector3(v0.x, v0.y, v1.z), new Vector3(v0.x, v1.y, v1.z), color, t);


        Debug.DrawLine(new Vector3(v1.x, v1.y, v0.z), new Vector3(v1.x, v1.y, v1.z), color, t);
        Debug.DrawLine(new Vector3(v1.x, v0.y, v1.z), new Vector3(v1.x, v1.y, v1.z), color, t);
        Debug.DrawLine(new Vector3(v0.x, v1.y, v1.z), new Vector3(v1.x, v1.y, v1.z), color, t);
    }
}
