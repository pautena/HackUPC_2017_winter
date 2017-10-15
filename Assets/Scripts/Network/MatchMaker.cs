using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

public class MatchMaker : NetworkBehaviour {

	public MyNetworkManager networkManager;
	public uint maximumMatchSize=100;

	// Use this for initialization
	void Start () {
		Connect ();
	}

	public void Connect(){
		networkManager.StartMatchMaker ();
		var matches = networkManager.matchMaker.ListMatches (0, 10, "", true, 0, 0, OnMatchList);
		Debug.Log ("matches: " + matches);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
	{
		Debug.Log ("success: " + success + ", matches: " + matches);
		Debug.Log ("extendedInfo: " + extendedInfo);
		if (success && matches.Count > 0) {
			JoinGame (matches [0].networkId);
		} else if (success) {
			CreateGame ();
		}
	}

	private void CreateGame(){
		networkManager.matchMaker.CreateMatch(networkManager.matchName,
			maximumMatchSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
	}

	private void JoinGame(NetworkID networkId){
		networkManager.matchMaker.JoinMatch(networkId, "" , "", "", 0, 0, networkManager.OnMatchJoined);
	}



}
