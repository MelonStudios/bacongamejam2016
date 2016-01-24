using UnityEngine;
using System.Collections;

public class BobBehaviour : MonoBehaviour {

    private float bobSpeed;
    private float bobHeight;
    private Vector3 startPos;
    private float counter;

	// Use this for initialization
	void Start () {
        bobSpeed = Random.Range(-2f, 2f);
        bobHeight = Random.Range(0.1f, 3f);
        startPos = transform.position;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime * bobSpeed;
        transform.position = startPos + Vector3.up * Mathf.Sin(counter) * bobHeight;
	}
}
