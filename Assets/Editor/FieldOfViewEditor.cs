using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI() {
        FieldOfView fov = (FieldOfView) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector2.up, 360, fov.viewRadius);
        Vector2 viewAngleA = fov.DirectionFromAngle(-fov.viewAngle/2, false);
        Vector2 viewAngleB = fov.DirectionFromAngle(fov.viewAngle/2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + (Vector3) viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + (Vector3) viewAngleB * fov.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}
