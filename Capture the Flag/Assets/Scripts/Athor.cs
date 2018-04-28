using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Athor : NetworkBehaviour {


	/// ASSIGN BY BUTTON For Testing///

/*	void Update () {
		if (isLocalPlayer) {

			// Assign Authority by pressing H when ball is next to player inside the trigger zone.
			if (Input.GetKeyDown (KeyCode.H)) {
			
				GameObject ObjectToAssign = GameObject.FindGameObjectWithTag ("Ball");
				CmdSetAuthority(ObjectToAssign.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());

			}

			// Removes Authority by pressing J when ball is next to player.
			if (Input.GetKeyDown (KeyCode.J)) {

				GameObject ObjectToAssign = GameObject.FindGameObjectWithTag ("Ball");
				CmdRemoveAuthority(ObjectToAssign.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());

			}
		}
	} */


	/// TRIGGER ZONE START///

	public void OnTriggerStay(Collider other)
	{
	
	CmdSetAuthority(other.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());

	}

	public void OnTriggerExit(Collider other)
	{
	
	CmdRemoveAuthority(other.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());

	}
		

	/// ASSIGN AND REMOVE CLIENT AUTHORITY///

	[Command]
	void CmdSetAuthority (NetworkIdentity grabID, NetworkIdentity playerID) {
		grabID.AssignClientAuthority (connectionToClient);
	}

	[Command]
	void CmdRemoveAuthority (NetworkIdentity grabID, NetworkIdentity playerID) {
		grabID.RemoveClientAuthority (connectionToClient);
	}
	
}
