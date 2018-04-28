using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {
	public Camera cam;
	public Text result;
	public Text place;
	public Text time;
	public float velocity = 5;
	public GameObject speed;
	public GameObject health;
	[SyncVar]
	public bool powerspeed;
	[SyncVar]
	public bool powershield;
	[SyncVar]
	public bool start = false;
	[SyncVar]
	public bool done = false;
	public int timeleft = 120;
	public int counter;
	[SyncVar]
	public bool hasFlag = false;
	[SyncVar]
	public int score = 0;
	public GameObject flag;
	public int maxPlayers = 2;
	public int playerColor;
	public GameObject bulletPrefab;
	public Transform bulletspawn;
	[SyncVar]
	public int spawnflag;
	// Update is called once per frame
	void FixedUpdate()
	{
		if (done) {
			if (place.text == "1st") {
				result.text = "Victory";
			} else {
				result.text = "Defeat";
			}
		}
		place.text = "1st";
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			if (g.GetComponent<PlayerController> ().score > score) {
				place.text = "2nd";
			}
		}
		counter++;
		if (counter >= 60) {
			//CmdSpawnPowerUps ();
			if (hasFlag) {
				score += 10;

			}
			if (start) {
				timeleft -= 1;
				time.text = "" + timeleft;
				if (timeleft <= 0) {
					done = true;
				}
			}

			counter = 0;

		}
	}
	void Update () {
		if (powerspeed) {
			velocity = 30;
		} else {
			velocity = 5;
		}
		if (hasFlag) {
			GetComponent<ParticleSystemRenderer>().material = GetComponent<MeshRenderer> ().material;
		}
		UpdateMaterials ();
		if (!isLocalPlayer) 
		{
			//gameObject.tag = "Dead";
			cam.enabled = false;
			return;
		}
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 250.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * velocity;

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			CmdFire ();
		}

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

	}
	void UpdateMaterials()
	{
		foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player")) {

			switch (n.GetComponent<PlayerController>().spawnflag) {
			case 0:
				n.GetComponent<MeshRenderer> ().material.color = Color.red;
				break;
			case 1:
				n.GetComponent<MeshRenderer> ().material.color = Color.blue;
				break;
			case 2:
				n.GetComponent<MeshRenderer> ().material.color = Color.green;
				break;
			case 3:
				n.GetComponent<MeshRenderer> ().material.color = Color.yellow;
				break;
			}
		}
	}
	public override void OnStartLocalPlayer()
	{
		GetComponent<ParticleSystem> ().Pause ();

		GetComponent<MeshRenderer> ().material.color = Color.blue;
		Transform spawnpoint = SpawnOverHead.spawn ();
		transform.position = spawnpoint.position;
		GameObject cflag = GameObject.FindGameObjectWithTag ("Flag");
		if (cflag == null && GameObject.FindGameObjectsWithTag("Player").Length == maxPlayers) {
			StartCoroutine (timeLimit());
			start = true;
			CmdStart ();
			CmdSpawnSpeed ();
			CmdspawnObjective ();
			CmdSpawnPowerUps ();

		}
		CmdspawnFlagupdate(spawnpoint.gameObject.GetComponent<SpawnPointScript> ().flag);
		//playerColor = Random.Range(1
	}
	[Command]
	void CmdSpawnPowerUps()
	{
		if (GameObject.FindGameObjectWithTag ("Heal") == null) {
			var sh = (GameObject)Instantiate (health, GameObject.FindGameObjectWithTag("HealSpawn").transform);
			NetworkServer.Spawn (sh);
		}

	}
	[Command]
	void CmdSpawnSpeed()
	{
		if (GameObject.FindGameObjectWithTag ("Speed") == null) {
			var sp = (GameObject)Instantiate (speed, GameObject.FindGameObjectWithTag("Speedspawn").transform);
			NetworkServer.Spawn (sp);
		}
	}
	[Command]
	void CmdStart()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			g.GetComponent<PlayerController> ().start = true;
		}
	}
	[Command]
	void CmdspawnObjective()
	{
		var mflag = (GameObject)Instantiate (flag, GameObject.FindGameObjectWithTag("FlagStart").transform);
		//NetworkServer.SpawnWithClientAuthority (mflag, gameObject);
		NetworkServer.Spawn (mflag);
	}
	[Command]
	void CmdspawnFlagupdate(int flag)
	{
		spawnflag = flag;
	}
	[Command]
	void CmdFire()
	{
		//Create the bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			             bulletPrefab,
			             bulletspawn.position,
			             bulletspawn.rotation);

		//Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		//Spawn the bullet on the clients
		NetworkServer.Spawn(bullet);
		//Destroy the bullet after 2 seconds
		Destroy(bullet,2.0f);
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Flag") 
		{
			GetComponent<ParticleSystem> ().Play ();
			GetComponent<PlayerController> ().hasFlag = true;
			if (!isLocalPlayer) {
				RpcParticleSys ();
			}
			CmdDespawnFlag (c.gameObject);
		}
		if (c.gameObject.tag == "Heal") {
			CmdSetShield ();
			StartCoroutine (shieldLimit ());
			CmdDespawnPickup (c.gameObject);
		}
		if (c.gameObject.tag == "Speed") {
			CmdSetSpeed ();
			StartCoroutine (shieldLimit ());
			CmdDespawnPickup (c.gameObject);
		}

	}
	[ClientRpc]
	void RpcParticleSys()
	{
		GetComponent<ParticleSystem> ().Play ();
		GetComponent<PlayerController> ().hasFlag = true;
	}
	[Command]
	void CmdDespawnFlag(GameObject g) {
		GetComponent<ParticleSystem> ().Play ();
		GetComponent<PlayerController> ().hasFlag = true;
		NetworkServer.Destroy (g);
	}
	[Command]
	void CmdDespawnPickup(GameObject g)
	{
		NetworkServer.Destroy (g);
	}
	IEnumerator timeLimit()
	{
		yield return new WaitForSeconds(120);
		start = false;
		CmdDisplayResults ();
	}
	[Command]
	void CmdDisplayResults()
	{
		GameObject winner = gameObject;
		winner.GetComponent<PlayerController> ().result.text = "Victory";
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			if (g.GetComponent<PlayerController> ().score > gameObject.GetComponent<PlayerController> ().score) {
				winner = g;
			}
			g.GetComponent<PlayerController> ().result.text = "Defeat";
		}
		
	}
	[Command]
	void CmdSetShield()
	{
		powershield = true;
	}
	[Command]
	void CmdSetSpeed()
	{
		powerspeed = true;
	}
	IEnumerator shieldLimit()
	{
		yield return new WaitForSeconds (30);
		powershield = false;
	}

	IEnumerator speedLimit()
	{
		yield return new WaitForSeconds (4);
		powerspeed = false;
	}
}
