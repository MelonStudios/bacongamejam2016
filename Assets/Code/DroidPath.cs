using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DroidPath : MonoBehaviour
{
    public List<Vector3> PathPoints;

    public int CurrentPointIndex { get { return currentPointIndex; } }
    public int NextPointIndex
    {
        get { return currentPointIndex + 1 < PathPoints.Count ? currentPointIndex + 1 : 0; }
    }

    public Vector3 CurrentPoint { get { return PathPoints[CurrentPointIndex]; } }
    public Vector3 NextPoint { get { return PathPoints[NextPointIndex]; } }

    private int currentPointIndex;

    public Vector3 GetAndSetNextPointIndex()
    {
        currentPointIndex = NextPointIndex;
        return CurrentPoint;
    }

    void Start()
    {
        currentPointIndex = 0;
    }

    void OnDrawGizmos()
    {
        if (PathPoints.Count < 1) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < PathPoints.Count; i++)
        {
            Gizmos.DrawCube(PathPoints[i], Vector3.one * 1.8f);

            if (PathPoints.Count > i + 1)
                Gizmos.DrawLine(PathPoints[i], PathPoints[i + 1]);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(PathPoints.Last(), PathPoints.First());
    }
}