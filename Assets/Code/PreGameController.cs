using UnityEngine;
using System.Collections;

public class PreGameController : MonoBehaviour
{
    public float CameraAnimationTime;

    private bool doOnce;

    void Start()
    {
        doOnce = true;
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.PreGame) return;

        if (doOnce)
        {
            doOnce = false;
            StartCoroutine(AnimateCamera());
        }
    }

    IEnumerator AnimateCamera()
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
        
        float deltaTime = 0;
        
        do
        {
            deltaTime += Time.deltaTime;

            cam.orthographicSize = Mathf.SmoothStep(originalSize, targetSize, MathUtility.PercentageBetween(deltaTime, 0, CameraAnimationTime));
            cam.transform.position = Vector3.Slerp(originalPosition, targetPosition, MathUtility.PercentageBetween(deltaTime, 0, CameraAnimationTime));
            cam.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, MathUtility.PercentageBetween(deltaTime, 0, CameraAnimationTime));

            yield return new WaitForEndOfFrame();
        } while (deltaTime <= CameraAnimationTime);

        GameInformation.Instance.GameState = GameState.Playing;
    }
}