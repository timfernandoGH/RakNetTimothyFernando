using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject flag = GameObject.FindGameObjectWithTag ("Flag");
		if (flag != null) {
			transform.LookAt (flag.transform);
		}
	}
}
