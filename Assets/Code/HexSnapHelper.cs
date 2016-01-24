using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class HexSnapHelper
{
    private static GameObject HexTemplate;

    public const float XOffSet = 1.0f;
    public const float YOffSet = 1.0f;
    public const float ZOffSet = 1.0f;

    static HexSnapHelper()
    {
        HexTemplate = (GameObject)Resources.Load("HexTemplate");
    }

    public static Vector3 CalculateNearestSnapLocation(Vector3 inputVector)
    {
        float hexPoint = HexTemplate.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        float hexFlat = HexTemplate.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.z;

        var hexFlatHalf = (hexFlat / 2);
        var hexPointHalf = (hexPoint / 2);
        var hexPointQuart = (hexPoint / 4);

        int flatFloor = Mathf.FloorToInt(inputVector.x / hexFlat);
        int pointFloor = Mathf.FloorToInt(inputVector.z / (hexPointHalf + hexPointQuart));

        float x, y, z;

        if (pointFloor % 2 == 0) // Even row
        {
            x = flatFloor * hexFlat * XOffSet;
        }
        else // Odd row
        {
            x = (flatFloor * hexFlat * XOffSet) + hexFlatHalf;
        }

        y = Mathf.Floor(inputVector.y) * YOffSet;
        z = pointFloor * (hexPointHalf + hexPointQuart) * ZOffSet;

        return new Vector3(x, y, z);
    }
}