using System.Collections;
using System.Collections.Generic;
using Unity;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SightComponent))]
public class SightComponentEditor : Editor
{
    protected virtual private void OnSceneGUI()
    {
        SightComponent sight = (SightComponent) target;
        float size = HandleUtility.GetHandleSize(sight.transform.position) * 10f;
        float snap = 0.1f;
        EditorGUI.BeginChangeCheck();
        Handles.color = new Color(0, 0, 1, 0.3f);
        Handles.DrawSolidDisc(sight.transform.position, new Vector3(0, 1, 0), sight.Far);
        Handles.color = new Color(0, 1, 0, 0.3f);
        Handles.DrawSolidDisc(sight.transform.position, new Vector3(0, 1, 0), sight.Medium);
        Handles.color = new Color(1, 0, 0, 0.3f);
        Handles.DrawSolidDisc(sight.transform.position, new Vector3(0, 1, 0), sight.Close);

    }
}