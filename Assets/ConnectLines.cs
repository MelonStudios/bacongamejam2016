using UnityEngine;
using System.Collections;

public class ConnectLines : MonoBehaviour {
    public LineRenderer line;
    public Transform endPoint;
	
	// Update is called once per frame
	void Update () {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, endPoint.position);
	}
}
