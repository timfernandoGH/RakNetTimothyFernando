using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerResults : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator timeLimit()
	{
		yield return new WaitForSeconds(20);
		DisplayResults ();
	}

	void DisplayResults()
	{
		
	}
}
