using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Countdown : NetworkBehaviour {

	public TextMesh textMesh;
	public float iterationSeconds=30f;
	public float rotationVelocity=6f;

	[SyncVar]
	private float time;
	public PoiManager poiManager;
	public Text textUI;

	// Use this for initialization
	void Start () {
		InitializeTime ();
	}

	private void InitializeTime(){
		time = iterationSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		DecTime ();

		if (time < 0f) {
			poiManager.DealPoints ();
			InitializeTime ();
		}
		PrintTime ();
		textMesh.gameObject.transform.Rotate (Vector3.up * Time.deltaTime * rotationVelocity);

	}
		
	[ServerCallback]
	private  void DecTime(){
		time -= Time.deltaTime;
	}
		
	private void PrintTime(){
		int minutes = (int)time/60;
		int seconds = (int)time % 60;
		string text = minutes.ToString("00") + ":" + seconds.ToString("00");
		textMesh.text = text;
		textUI.text = text;
	}
}
