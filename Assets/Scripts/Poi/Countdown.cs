using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Countdown : MonoBehaviour {

	private TextMesh textMesh;
	public float iterationSeconds=30f;
	public float rotationVelocity=6f;
	private float time;

	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh> ();
		InitializeTime ();
	}

	private void InitializeTime(){
		time = iterationSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;

		if (time < 0f) {
			InitializeTime ();
		}
		PrintTime ();
		transform.Rotate (Vector3.up * Time.deltaTime * rotationVelocity);

	}

	private void PrintTime(){
		int minutes = (int)time/60;
		int seconds = (int)time % 60;
		string text = minutes.ToString("00") + ":" + seconds.ToString("00");
		textMesh.text = text;

	}
}
