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
        StartCoroutine(PointToMouse());
    }

    void LateUpdate()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;
        if (playerInformation.CharacterState != CharacterState.Alive) return;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        rigid.AddForce(CalculateMovement(cameraController.CameraDirection, horizontal, vertical) * playerInformation.Speed);
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

    IEnumerator PointToMouse()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (GameInformation.Instance.GameState != GameState.Playing) continue;
            if (playerInformation.CharacterState != CharacterState.Alive) continue;

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            // PlayerLooktoMouseCollider = 8
            if (Physics.Raycast(mouseRay, out mouseHit, 1000, 1 << 8))
            {
                Quaternion originalRotation = transform.rotation;
                transform.LookAt(new Vector3(mouseHit.point.x, transform.position.y, mouseHit.point.z));
                Quaternion targetRotation = transform.rotation;
                transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, Time.deltaTime * playerInformation.RotationSpeed);
            }
        }
    }
}