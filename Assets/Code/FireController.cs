using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour
{
    public GameObject Bullet;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(Bullet, transform.position, transform.rotation);
        }
    }
}