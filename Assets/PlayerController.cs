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
        Vector3 movement = new Vector3(horizontal + vertical, 0, vertical - horizontal);
        rigidbody.AddForce(movement * 50f);
    }
}