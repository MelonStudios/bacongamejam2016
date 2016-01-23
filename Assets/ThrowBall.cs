using UnityEngine;
using System.Collections;

public class ThrowBall : MonoBehaviour {

    Rigidbody rigidBody;
    MeshRenderer meshRenderer;
    public GameObject player;
    bool thrown = false;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        meshRenderer= GetComponent<MeshRenderer>();

        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {

        // INPUT
        if (Input.GetButtonDown("Fire1"))
        {
            if(!thrown)
            {
                Throw();
                thrown = true;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            thrown = false;
        }

        // UPDATE ACTIONS
	    if(!thrown)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
        }
	}

    void Throw()
    {
        transform.position = Camera.main.transform.position;
        rigidBody.AddForce(Camera.main.transform.forward * 1000f);
    }
}
