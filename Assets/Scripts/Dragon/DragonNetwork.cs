using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DragonNetwork : NetworkBehaviour {
	public DragonController dragonController;
	public DragoFire dragoFire;
	public CapsuleCollider healthCollider;
	public Camera camera;
	public SkinnedMeshRenderer renderer;

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
	}

	private void Enable(){
		dragonController.enabled = true;
		dragoFire.enabled = true;
		camera.gameObject.SetActive(true);
		healthCollider.enabled = true;
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
	}
	private void SetMaterial(int team){
		MyNetworkManager networkManager = GameObject.FindGameObjectWithTag ("NetworkManager")
			.GetComponent<MyNetworkManager> ();
		Material material = networkManager.GetMaterial (team);
		renderer.material = material;
	}
		
}
