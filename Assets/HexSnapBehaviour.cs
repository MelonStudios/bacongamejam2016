using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HexSnapBehaviour : MonoBehaviour
{
    float hexFlat;
    float hexPoint;

    public const float XOffSet = 1.0f;
    public const float YOffSet = 1.0f;
    public const float ZOffSet = 1.0f;
    
    void Update()
    {
        hexPoint = GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
        hexFlat = GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.z;

        var hexFlatHalf = (hexFlat / 2);
        var hexPointHalf = (hexPoint / 2);
        var hexPointQuart = (hexPoint / 4);

        int flatFloor = Mathf.FloorToInt(transform.position.x / hexFlat);
        int pointFloor = Mathf.FloorToInt(transform.position.z / (hexPointHalf + hexPointQuart));

        float x, y, z;

        if (pointFloor % 2 == 0) // Even row
        {
            x = flatFloor * hexFlat * XOffSet;
        }
        else // Odd row
        {
            x = (flatFloor * hexFlat * XOffSet) + hexFlatHalf;
        }

        y = transform.position.y * YOffSet;
        z = pointFloor * (hexPointHalf + hexPointQuart) * ZOffSet;

        transform.position = new Vector3(x, y, z);
    }
}