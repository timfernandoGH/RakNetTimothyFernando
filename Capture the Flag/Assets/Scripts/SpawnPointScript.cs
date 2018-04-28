using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SpawnPointScript : NetworkBehaviour {
	[SyncVar]
	public bool holdingPlayer;
	public int flag;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider c)
	{
		CmdUpdateHold (true);
	}
	void OnTriggerExit(Collider c)
	{
		CmdUpdateHold (false);
	}
	[Command]
	public void CmdUpdateHold(bool ishold)
	{
		holdingPlayer = ishold;
	}
}
