using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BillboardScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main != null) {
			transform.LookAt (Camera.main.transform);
		}
	}
}
