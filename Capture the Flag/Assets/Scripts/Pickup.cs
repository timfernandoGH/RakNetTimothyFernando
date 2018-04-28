using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Pickup : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider c)
	{
		Debug.Log ("here");
		if (c.gameObject.tag == "Player") 
		{
			if (gameObject.tag != "Flag") {
				
			} 
			else 
			{
				c.GetComponent<ParticleSystem> ().Play ();
				c.GetComponent<PlayerController> ().hasFlag = true;
				CmdDespawnFlag (netId);


			}
		}
	}
	[Command]
	void CmdDespawnFlag(NetworkInstanceId id) {
		GameObject theObject = NetworkServer.FindLocalObject (id);
		NetworkServer.Destroy (theObject);
	}
}
