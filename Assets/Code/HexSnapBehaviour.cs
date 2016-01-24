using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HexSnapBehaviour : MonoBehaviour
{
    void Update()
    {
        transform.position = HexSnapHelper.CalculateNearestSnapLocation(transform.position);
    }
}