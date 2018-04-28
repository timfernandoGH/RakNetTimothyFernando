using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class HealthScript : NetworkBehaviour {
	public const int maxHealth = 100;
	public GameObject flag;
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
	public RectTransform healthbar;

	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}
		if (!GetComponent<PlayerController> ().powershield) {
			currentHealth -= amount;
		}
		if (currentHealth <=0) 
		{
			currentHealth = maxHealth;
			RpcRespawn ();
			if (GetComponent<PlayerController> ().hasFlag) {
				GetComponent<PlayerController> ().hasFlag = false;
				CmdspawnObjective ();
			}
		}

	}
	void OnChangeHealth(int health)
	{
		healthbar.sizeDelta = new Vector2 (currentHealth, healthbar.sizeDelta.y);
	}
	[Command]
	void CmdspawnObjective()
	{
		
		var mflag = (GameObject)Instantiate (flag, GameObject.FindGameObjectWithTag("FlagStart").transform);
		//NetworkServer.SpawnWithClientAuthority (mflag, gameObject);
		NetworkServer.Spawn (mflag);
	}
	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer) 
		{
			GetComponent<ParticleSystem> ().Stop ();
			currentHealth = maxHealth;
			Transform spawnpoint = SpawnOverHead.spawn ();
			transform.position = spawnpoint.position;
		}
	}
}
