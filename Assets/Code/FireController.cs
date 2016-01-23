using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour
{
    public GameObject FirePoint;
    public GameObject Bullet;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(Bullet, FirePoint.transform.position, transform.rotation);
        }
    }
}