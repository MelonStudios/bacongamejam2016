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

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<HexTileInformation>() != null)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Destroy(BulletMesh);
            Destroy(gameObject, 0.5f);
        }
        else if (other.GetComponentInParent<PlayerInformation>() != null)
        {
            other.GetComponentInParent<PlayerInformation>().PlayerState = PlayerState.Dead;
        }
    }
}