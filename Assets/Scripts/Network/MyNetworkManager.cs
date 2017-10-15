using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

	public GameObject myPlayerPrefab;
	private int nPlayers = 0;
	public int numTeams=4;
	public Material[] playerMaterials;

	private int GetTeam(){
		return nPlayers % numTeams;
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);
		Debug.Log ("OnClientConnect");
	}
		


	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		Debug.Log ("OnServerAddPlayer");
		nPlayers++;
		int team = GetTeam ();
		if (myPlayerPrefab == null)
		{
			if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
			return;
		}

		if (myPlayerPrefab.GetComponent<NetworkIdentity>() == null)
		{
			if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
			return;
		}

		if (playerControllerId < conn.playerControllers.Count  && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
		{
			if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
			return;
		}

		GameObject player;
		Transform startPos = GetStartPosition();
		if (startPos != null)
		{
			player = (GameObject)Instantiate(myPlayerPrefab, startPos.position, startPos.rotation);
		}
		else
		{
			player = (GameObject)Instantiate(myPlayerPrefab, Vector3.zero, Quaternion.identity);
		}

		DragonNetwork dragonNetwork = player.GetComponent<DragonNetwork> ();
		dragonNetwork.SyncTeam (team);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public Material GetMaterial(int team){
		return playerMaterials[team];
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		base.OnClientDisconnect (conn);
		GetComponent<MatchMaker> ().Connect ();
	}

}
