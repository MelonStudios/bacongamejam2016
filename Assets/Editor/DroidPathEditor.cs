using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DroidPath))]
class DroidPathEditor : Editor
{
    DroidPath path;
    GUIStyle labelStyle = new GUIStyle();

    void OnEnable()
    {
        path = (DroidPath)target;

        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.fontSize = 24;
        labelStyle.normal.textColor = Color.green;
    }

    public override void OnInspectorGUI()
    {
        if (path.PathPoints == null) path.PathPoints = new List<Vector3>();

        GUI.enabled = !Application.isPlaying;
        if (GUILayout.Button("Add Path Point")) path.PathPoints.Add(Vector3.zero);

        ListPathPoints(ref path.PathPoints);

        GUI.enabled = true;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void OnSceneGUI()
    {
        Undo.RecordObject(path, "Undo Droid Path Move");

        for (int i = 0; i < path.PathPoints.Count; i++)
        {
            var targetPos = HexSnapHelper.CalculateNearestSnapLocation(Handles.PositionHandle(path.PathPoints[i], Quaternion.identity));
            targetPos.y = path.transform.position.y;
            path.PathPoints[i] = targetPos;
            Handles.Label(path.PathPoints[i], i.ToString(), labelStyle);
        }
    }

    private void ListPathPoints(ref List<Vector3> points)
    {
        EditorGUILayout.LabelField("Points");

        GUI.enabled = false;
        for (int i = 0; i < points.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Vector3Field(i.ToString(), points[i]);
            GUI.enabled = true;
            if (GUILayout.Button("-", GUILayout.ExpandWidth(false))) points.RemoveAt(i);
            GUI.enabled = false;
            EditorGUILayout.EndHorizontal();
        }
        GUI.enabled = true;
    }
}