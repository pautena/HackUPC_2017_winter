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

		string matchName = networkManager.matchName;
		Debug.Log ("ListMatches -> matchName: " + matchName);

		var matches = networkManager.matchMaker.ListMatches (0, 10, matchName, true, 0, 0, OnMatchList);
		Debug.Log ("matches: " + matches);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
	{
		Debug.Log ("success: " + success + ", matches: " + matches.Count);
		Debug.Log ("extendedInfo: " + extendedInfo);
		foreach (MatchInfoSnapshot match in matches) {
			Debug.Log (match.name);
		}
		if (success && matches.Count > 0) {
			JoinGame (matches [0].networkId);
		} else if (success) {
			CreateGame ();
		}
	}

	private void CreateGame(){
		string matchName = networkManager.matchName;
		Debug.Log ("CreateGame -> matchName: " + matchName);
		networkManager.matchMaker.CreateMatch(matchName,
			maximumMatchSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
	}

	private void JoinGame(NetworkID networkId){
		Debug.Log ("Joint to " + networkId);
		networkManager.matchMaker.JoinMatch(networkId, "" , "", "", 0, 0, networkManager.OnMatchJoined);
	}



}
