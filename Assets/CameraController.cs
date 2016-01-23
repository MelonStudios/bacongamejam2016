using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraDirection _cameraDirection;

    public CameraDirection CameraDirection
    {
        get { return _cameraDirection; }
        set { _cameraDirection = value; }
    }

    private float _cameraDistance;

    public float CameraDistance
    {
        get { return _cameraDistance; }
        set { _cameraDistance = Mathf.Floor(value); }
    }

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CameraDistance = 5f;
    }

    void Update()
    {

        Debug.DrawRay((CameraDirectionToRayVector(CameraDirection) * CameraDistance) + player.transform.position, new Vector3(0, 1, 0), Color.red);

        Debug.DrawRay(player.transform.position, CameraDirectionToRayVector(CameraDirection.North), Color.red);
        Debug.DrawRay(player.transform.position, CameraDirectionToRayVector(CameraDirection.East), Color.green);
        Debug.DrawRay(player.transform.position, CameraDirectionToRayVector(CameraDirection.South), Color.blue);
        Debug.DrawRay(player.transform.position, CameraDirectionToRayVector(CameraDirection.West), Color.yellow);
    }

    private Vector3 CameraDirectionToRayVector(CameraDirection cameraDirection)
    {
        switch (cameraDirection)
        {
            case CameraDirection.North:
                return new Vector3(-1, 1, -1);
            case CameraDirection.East:
                return new Vector3(1, 1, -1);
            case CameraDirection.South:
                return new Vector3(1, 1, 1);
            case CameraDirection.West:
                return new Vector3(-1, 1, 1);
            default:
                throw new System.Exception("fail");
        }
    }
}

public enum CameraDirection
{
    North,
    East,
    South,
    West
}