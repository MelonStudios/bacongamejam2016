using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    private CameraController cameraController;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        rigidbody.AddForce(CalculateMovement(cameraController.CameraDirection, horizontal, vertical) * 50f);
    }

    private Vector3 CalculateMovement(CameraDirection cameraDirection, float hoz, float vert)
    {
        switch (cameraDirection)
        {
            case CameraDirection.North:
                return new Vector3(hoz + vert, 0, vert - hoz);
            case CameraDirection.East:
                return new Vector3(hoz - vert, 0, vert + hoz);
            case CameraDirection.South:
                return new Vector3(vert + hoz, 0, hoz + vert);
            case CameraDirection.West:
                return new Vector3(vert + hoz, 0, vert - hoz);
            default:
                throw new System.Exception("Bad Camera Direction");
        }
    }
}