using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerInformation), typeof(FireController), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private PlayerInformation playerInformation;
    private Rigidbody rigid;
    private CameraController cameraController;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        playerInformation = GetComponent<PlayerInformation>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    void LateUpdate()
    {
        if (playerInformation.PlayerState == PlayerState.Alive)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            rigid.AddForce(CalculateMovement(cameraController.CameraDirection, horizontal, vertical) * playerInformation.Speed);

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            // PlayerLooktoMouseCollider = 8
            if (Physics.Raycast(mouseRay, out mouseHit, 1000, 1 << 8))
            {
                transform.LookAt(new Vector3(mouseHit.point.x, transform.position.y, mouseHit.point.z));
            }
        }
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
                return new Vector3(-hoz - vert, 0, hoz - vert);
            case CameraDirection.West:
                return new Vector3(vert - hoz, 0, -vert - hoz);
            default:
                throw new System.Exception("Bad Camera Direction");
        }
    }
}