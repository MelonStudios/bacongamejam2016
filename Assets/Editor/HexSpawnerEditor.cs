using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(HexSpawner))]
    class HexSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("GenHexes"))
            {
                HexSpawner.Instance.GenHexes();
            }

            GUI.enabled = true;

            base.OnInspectorGUI();
        }
    }
}
