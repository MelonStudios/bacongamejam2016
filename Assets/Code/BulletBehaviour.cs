using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBehaviour : MonoBehaviour
{
    public float LifeTime;
    public float BulletSpeed;
    public float RotateSpeed;

    public GameObject BulletMesh;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * BulletSpeed, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddRelativeTorque(Vector3.one * RotateSpeed);

        Destroy(gameObject, LifeTime);
    }

    void Update()
    {
    }
}