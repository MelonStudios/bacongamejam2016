using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DroidPath : MonoBehaviour
{
    public List<Vector3> PathPoints;

    public int PreviousPointIndex
    {
        get { return nextPointIndex - 1 < 0 ? PathPoints.Count - 1 : nextPointIndex - 1; }
    }

    public int NextPointIndex { get { return nextPointIndex; } }

    public Vector3 PreviousPoint { get { return PathPoints[PreviousPointIndex]; } }
    public Vector3 NextPoint { get { return PathPoints[NextPointIndex]; } }

    private int nextPointIndex;

    public Vector3 GetAndSetNextPointIndex()
    {
        nextPointIndex = nextPointIndex + 1 < PathPoints.Count ? nextPointIndex + 1 : 0;
        return NextPoint;
    }

    void Start()
    {
        nextPointIndex = 0;
    }

    void OnDrawGizmosSelected()
    {
        if (PathPoints.Count < 1) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < PathPoints.Count; i++)
        {
            if (PathPoints.Count > i + 1)
                Gizmos.DrawLine(PathPoints[i], PathPoints[i + 1]);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(PathPoints.Last(), PathPoints.First());

        Gizmos.color = Color.green;
        for (int i = 0; i < PathPoints.Count; i++)
        {
            Gizmos.DrawCube(PathPoints[i], Vector3.one * 1.8f);
        }
    }
}