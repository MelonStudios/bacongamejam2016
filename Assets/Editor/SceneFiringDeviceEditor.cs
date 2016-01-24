using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneFiringDevice))]
class SceneFiringDeviceEditor : Editor
{
    Vector3 origin = Vector3.zero;
    void OnSceneGUI()
    {
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.MouseMove:
                {
                    Debug.Log("MOUSE MOVE");
                    FireResults points = CalculateFirePoints();

                    foreach (var point in points)
                    {
                        Gizmos.DrawLine(point.Origin, point.Ending);
                    }
                }
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.S)
                {
                    Debug.Log("KEY DOWN S");
                    origin = GetMousePoint();
                }
                break;
        }
    }

    public Vector3 GetMousePoint()
    {
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = Camera.current.pixelHeight - mousePos.y;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit mouseHit;

        // PlayerLooktoMouseCollider = 8
        if (Physics.Raycast(mouseRay, out mouseHit, 1000, 1 << 8))
        {
            return mouseHit.point;
        }

        return Vector3.zero;
    }


    /// <summary>
    /// Calculates all the fire points
    /// </summary>
    /// <returns>A list of point-heading tuples</returns>
    private FireResults CalculateFirePoints()
    {
        var firePoints = new FireResults();
        firePoints = GetNextPointRecursivly(ref firePoints, origin, GetMousePoint() - origin);

        return firePoints;
    }

    private FireResults GetNextPointRecursivly(ref FireResults currentFireResults, Vector3 origin, Vector3 heading)
    {
        if (currentFireResults.Bounces >= 50) return currentFireResults;

        RaycastHit hit;
        if (Physics.Raycast(origin, heading, out hit, 1000, 1 << 12)) // HexWall 12
        {
            HexType hexType = hit.collider.GetComponentInParent<HexTileInformation>().HexType;
            List<GameObject> hitEnemies = CalculateEnemyHits(origin, hit.point, heading);

            currentFireResults.Add(new FireSegment(origin, hit.point, hexType, hitEnemies));

            switch (hexType)
            {
                case HexType.Wall:
                    break;
                case HexType.Mirror:
                    currentFireResults = GetNextPointRecursivly(ref currentFireResults, hit.point, Vector3.Reflect(heading, hit.normal));
                    break;
            }
        }

        return currentFireResults;
    }

    private List<GameObject> CalculateEnemyHits(Vector3 origin, Vector3 ending, Vector3 direction)
    {
        float rayDistance = Vector3.Distance(origin, ending);
        return Physics.RaycastAll(origin, direction, rayDistance, 1 << 11).Select(rh => rh.collider.gameObject).ToList(); // Enemy layer 11
    }
}