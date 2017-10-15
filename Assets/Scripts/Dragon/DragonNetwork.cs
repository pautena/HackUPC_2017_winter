using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DragonNetwork : NetworkBehaviour {
	public DragonController dragonController;
	public DragoFire dragoFire;
	public CapsuleCollider healthCollider;
	public Camera camera;
	public SkinnedMeshRenderer renderer;
	public Slider healthSlider;

	[SyncVar(hook="SetTeam")]
	public int team=-1;

	// Use this for initialization
	void Start () {
		

		if (team != -1) {
			SetMaterial (team);
		}
		
		if (!isLocalPlayer) {
			Disable ();
			return;
		} else {
			Enable ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Disable(){
		dragonController.enabled = false;
		dragoFire.enabled = false;
		camera.gameObject.SetActive(false);
		healthCollider.enabled = false;
		healthSlider.gameObject.SetActive (true);
	}

	private void Enable(){
		dragonController.enabled = true;
		dragoFire.enabled = true;
		camera.gameObject.SetActive(true);
		healthCollider.enabled = true;
		healthSlider.gameObject.SetActive (false);
	}
		
	public void SyncTeam(int team){
		this.team = team;
		SetName (team);
		SetMaterial (team);
	}

	private void SetTeam(int team){
		SetName (team);
		SetMaterial (team);
	}

	private void SetName(int team){		
		gameObject.name = "Player_team" + team;
		Invoke ("SetUITeamName", 2);


	}

	private void SetUITeamName(){
		if (isLocalPlayer) {
			Text teamNameUI = GameObject.FindGameObjectWithTag ("TeamNameUI").GetComponent<Text> ();
			teamNameUI.text = "Team " + team;
		}
	}
	private void SetMaterial(int team){
		MyNetworkManager networkManager = GameObject.FindGameObjectWithTag ("NetworkManager")
			.GetComponent<MyNetworkManager> ();
		Material material = networkManager.GetMaterial (team);
		renderer.material = material;
	}
		
}
