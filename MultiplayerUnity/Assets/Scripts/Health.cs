using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	private NetworkStartPosition[] spawnPoints;
	public const int maxHealth = 100;
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
	public RectTransform healthbar;
	public bool destroyOnDeath;
	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			if (destroyOnDeath) {
				Destroy (gameObject);
			} else {
				currentHealth = maxHealth;
				RpcRespawn ();
			}
		}
			
	}
	public void OnChangeHealth(int health)
	{
		healthbar.sizeDelta = new Vector2 (health, healthbar.sizeDelta.y);
	}
	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer) 
		{
			// move back to zero location
			Vector3 spawnPoint = Vector3.zero;

			//If there is a spawn point array and the array is not empty, pick one at random
			if (spawnPoints != null && spawnPoints.Length > 0) 
			{
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}

			transform.position = spawnPoint;
		}
		
	}
}
