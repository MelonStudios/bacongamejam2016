﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VisualEffectController))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public VisualEffectController VisualEffectController { get; private set; }

    public CameraDirection startCameraDirection;

    public float RotateAnimationTime;

    private CameraDirection _cameraDirection;
    private const float cameraPhysicalDistance = 30;

    public CameraDirection CameraDirection
    {
        get { return _cameraDirection; }
        set { _cameraDirection = value; }
    }
    
    [Range(0, 2)]
    public float CameraDistanceLerpTime;
    
    public int TargetCameraDistance;

    public int MinCameraDistance;
    public int MaxCameraDistance;

    private GameObject player;

    private bool doOnce;

    void Reset()
    {
        TargetCameraDistance = 20;
        CameraDistanceLerpTime = 0.25f;
        MinCameraDistance = 8;
        MaxCameraDistance = 40;
    }

    void Start()
    {
        Instance = this;
        VisualEffectController = GetComponent<VisualEffectController>();
        player = GameObject.FindGameObjectWithTag("Player");
        CameraDirection = startCameraDirection;

        doOnce = true;
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;
        if (player.GetComponent<PlayerInformation>().CharacterState != CharacterState.Alive) return;

        if (doOnce)
        {
            doOnce = false;
            StartCoroutine(CameraDistanceController());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CameraDirection = SwitchCameraDirection(CameraDirection, true);
            StartCoroutine(RotateCamera());
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CameraDirection = SwitchCameraDirection(CameraDirection, false);
            StartCoroutine(RotateCamera());
            return;
        }

        if (player.gameObject != null)
        {
            transform.position = CalculateTargetPosition();
            transform.LookAt(player.transform);
        }

        if (Input.mouseScrollDelta != Vector2.zero)
            TargetCameraDistance -= Convert.ToInt32(Input.mouseScrollDelta.y);

        TargetCameraDistance = Mathf.Clamp(TargetCameraDistance, MinCameraDistance, MaxCameraDistance);

        DebugController.Instance.LogLine(string.Format("CAM DIRECTION: {0}", CameraDirection.ToString()));
        DebugController.Instance.LogLine(string.Format("CAM TARGET DISTANCE: {0}", TargetCameraDistance));
    }

    public Vector3 CalculateTargetPosition()
    {
        return (CameraDirectionToRayVector(CameraDirection) * cameraPhysicalDistance) + player.transform.position;
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
    
    IEnumerator RotateCamera()
    {
        Camera cam = Camera.main;
        Vector3 originalPosition = cam.transform.position;
        Vector3 targetPosition = cam.GetComponent<CameraController>().CalculateTargetPosition();
        float originalSize = cam.orthographicSize;
        float targetSize = cam.GetComponent<CameraController>().TargetCameraDistance;

        Vector3 playerPosition = GameInformation.Instance.PlayerInformation.transform.position;
        Quaternion originalRotation = cam.transform.rotation;

        cam.transform.position = targetPosition;
        cam.transform.LookAt(playerPosition);
        Quaternion targetRotation = cam.transform.rotation;
        cam.transform.position = originalPosition;
        cam.transform.rotation = originalRotation;

        GameInformation.Instance.GameState = GameState.Paused;
        Time.timeScale = 0;

        float deltaTime = 0;

        do
        {
            deltaTime += Time.unscaledDeltaTime;

            cam.orthographicSize = Mathf.SmoothStep(originalSize, targetSize, MathUtility.PercentageBetween(deltaTime, 0, RotateAnimationTime));
            cam.transform.position = Vector3.Lerp(originalPosition, targetPosition, MathUtility.PercentageBetween(deltaTime, 0, RotateAnimationTime));
            cam.transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, MathUtility.PercentageBetween(deltaTime, 0, RotateAnimationTime));

            yield return new WaitForEndOfFrame();
        } while (deltaTime <= RotateAnimationTime);

        Time.timeScale = 1;
        GameInformation.Instance.GameState = GameState.Playing;
    }

    IEnumerator CameraDistanceController()
    {
        float tempStart = 0;
        int tempTarget = 0;
        float time = 0;

        while (true)
        {
            if (tempTarget != TargetCameraDistance)
            {
                tempStart = GetComponent<Camera>().orthographicSize;
                tempTarget = TargetCameraDistance;
            }

            if (!Mathf.Approximately(GetComponent<Camera>().orthographicSize, TargetCameraDistance))
            {
                GetComponent<Camera>().orthographicSize = Mathf.SmoothStep(tempStart, tempTarget, MathUtility.PercentageBetween(time, 0, CameraDistanceLerpTime, true));

                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }

            yield return new WaitForEndOfFrame();
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