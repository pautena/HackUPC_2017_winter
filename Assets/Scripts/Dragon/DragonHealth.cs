using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DragonHealth : NetworkBehaviour {
	public float startHealth = 100.0f;


	[SyncVar(hook="HookHealth")]
	public float health;
	public Slider healthSlider;

	// Use this for initialization
	void Awake () {
		health = startHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(float damage){
		health -= damage;


		if (health < 0) {
			health = 0.0f;
			Die ();
		}
		UpdateUI ();
	}

	private void HookHealth(float newHealth){
		UpdateUI();
	}

	private void UpdateUI(){
		healthSlider.value = health / startHealth;
	}

	public void Die(){
		//TODO
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter: " + other);
		Fireball fireball = other.gameObject.GetComponent<Fireball> ();

		if (fireball != null) {
			TakeDamage (fireball.Force);
		}
	}
}
