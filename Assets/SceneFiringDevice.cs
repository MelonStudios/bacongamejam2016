using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[ExecuteInEditMode]
public class SceneFiringDevice : MonoBehaviour
{
    public Vector3 origin;

    public int BounceLimit = 30;

    [MenuItem("My Commands/Select Firing Device %g")]
    static void SpecialCommand()
    {
        Selection.activeObject = FindObjectOfType<SceneFiringDevice>();
        Debug.Log("You used the shortcut Cmd+G (Mac)  Ctrl+G (Win)");
    }
}