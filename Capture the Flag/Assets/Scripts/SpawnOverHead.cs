using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnOverHead {
	public static Transform spawn()
	{
		GameObject[] spawnPoints = new GameObject[4];
	
		spawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoints");
		for (int i = 0; i < 4; i++) 
		{
			if (!spawnPoints [i].GetComponent<SpawnPointScript> ().holdingPlayer) 
			{
				spawnPoints [i].GetComponent<SpawnPointScript> ().CmdUpdateHold (true);
				return spawnPoints [i].transform;
			}
		}
		return spawnPoints [0].transform;
	}
}
