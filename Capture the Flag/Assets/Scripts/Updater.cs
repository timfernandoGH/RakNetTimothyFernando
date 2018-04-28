using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Updater : NetworkBehaviour {
	public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			GetComponent<SphereCollider> ().enabled = false;
			GetComponent<Rigidbody> ().velocity = (target.transform.position - transform.position).normalized * 3;
			//CmdUpdatePos (gameObject.transform.position);
		} else {
			GetComponent<SphereCollider> ().enabled = true;
		}
	}
	[Command]
	void CmdUpdatePos(Vector3 v)
	{
		gameObject.transform.position = v;
	}
}
