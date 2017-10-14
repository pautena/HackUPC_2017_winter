using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PoiManager : NetworkBehaviour {

	public Material defaultWallMaterial;
	public Material[] wallMaterials;

	public Material defaultPoiMaterial;
	public Material[] poiMaterials;

	public MeshRenderer wall;
	public MeshRenderer poi;
	public int[] teamPoints;
	public CapsuleCollider wallCollider;
	private MyNetworkManager networkManager;

	// Use this for initialization
	void Start () {
		networkManager = GameObject.FindGameObjectWithTag ("NetworkManager")
			.GetComponent<MyNetworkManager> ();
		InitializeTeamPoints ();
		
	}

	private void InitializeTeamPoints(){
		int numTeams = networkManager.numTeams;
		teamPoints = Enumerable.Repeat(0, numTeams).ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private int GetTeam(Collider collider){
		DragonNetwork dragonNetwork = collider.gameObject.GetComponent<DragonNetwork> ();
		return dragonNetwork.team;
	}

	private void CalculatePoints(){
		InitializeTeamPoints ();

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		Vector3 wallColliderPosition = new Vector3 (wallCollider.transform.position.x, 0f, wallCollider.transform.position.z);

		foreach (GameObject player in players) {
			Vector3 playerPosition = new Vector3 (player.transform.position.x, 0f, player.transform.position.z);

			float distance = Vector3.Distance (wallColliderPosition, playerPosition);
			float minDistance = Mathf.Sqrt (Mathf.Pow(wallCollider.bounds.extents.x,2) + Mathf.Pow(wallCollider.bounds.extents.z,2));

			if(distance< minDistance){
				DragonNetwork dragonNetwork = player.GetComponent<DragonNetwork> ();
				int team = dragonNetwork.team;
				teamPoints [team+1]++;
			}
		}
	}

	private int GetBeastTeam(){
		int bestTeam = -1;

		for (int team=0; team<teamPoints.Length;++team){
			int points = teamPoints [team];
			Debug.Log ("team: " + team + ", points: " + points);
			if (points > 0 && bestTeam == -1) {
				bestTeam = team;
			}else if (points > 0 && points > teamPoints [bestTeam]) {
				bestTeam = team;
			}

		}
		return bestTeam+1;
	}

	private void  UpdateColor(){
		int bestTeam = GetBeastTeam () -1;

		Material poiMaterial;
		Material wallMaterial;
		if (bestTeam > 0) {
			poiMaterial = poiMaterials [bestTeam];
			wallMaterial = wallMaterials [bestTeam];
		} else {
			poiMaterial = defaultPoiMaterial;
			wallMaterial = defaultWallMaterial;
		}

		wall.material = wallMaterial;
		poi.material = poiMaterial;
	}

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player") {
			int team = GetTeam (collider);
			Debug.Log ("PoiManager. OnTriggerEnter. collider: " + collider+", team: "+team);
			CalculatePoints ();
			UpdateColor ();
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.gameObject.tag == "Player") {
			int team = GetTeam (collider);
			Debug.Log ("PoiManager. OnTriggerExit. collider: " + collider+", team: "+team);
			Invoke ("OnTriggerExitDelayed", 2);
		}
	}

	private void OnTriggerExitDelayed(){
		CalculatePoints ();
		UpdateColor ();
	}
}
