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
        CameraDistance = 30f;
    }

    void Update()
    {
        transform.position = (CameraDirectionToRayVector(CameraDirection) * CameraDistance) + player.transform.position;
        transform.LookAt(player.transform);
        Debug.DrawRay((CameraDirectionToRayVector(CameraDirection) * CameraDistance) + player.transform.position, new Vector3(0, 1, 0), Color.red);

        if (Input.GetKeyDown(KeyCode.E))
            CameraDirection = SwitchCameraDirection(CameraDirection, true);
        else if (Input.GetKeyDown(KeyCode.Q))
            CameraDirection = SwitchCameraDirection(CameraDirection, false);

        DebugController.Instance.LogLine(string.Format("CAM DIRECTION: {0}", CameraDirection.ToString()));
        DebugController.Instance.LogLine(string.Format("CAM DISTANCE: {0}", CameraDistance));
    }

    private CameraDirection SwitchCameraDirection(CameraDirection cameraDirection, bool clockwise)
    {
        switch (cameraDirection)
        {
            case CameraDirection.North:
                return clockwise ? CameraDirection.East : CameraDirection.West;
            case CameraDirection.East:
                return clockwise ? CameraDirection.South : CameraDirection.North;
            case CameraDirection.South:
                return clockwise ? CameraDirection.West : CameraDirection.East;
            case CameraDirection.West:
                return clockwise ? CameraDirection.North : CameraDirection.South;
            default:
                throw new System.Exception("swich cam direction fail");
        }
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