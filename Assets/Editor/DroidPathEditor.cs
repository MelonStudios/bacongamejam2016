using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(DroidPath))]
    class DroidPathEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DroidPath path = (DroidPath)target;

            GUI.enabled = !Application.isPlaying;
            if (GUILayout.Button("Add Path Point")) path.PathPoints.Add(Vector3.zero);

            ListPathPoints(ref path.PathPoints);

            GUI.enabled = true;
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
                EditorGUILayout.EndHorizontal();

                DrawHandle();
            }
            GUI.enabled = true;
        }

        private void DrawHandle()
        {

        }
    }
}
