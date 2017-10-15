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
	public float explosionStrength = 100f;

	public Animator animator;
	public Rigidbody rb;


	void Start(){
		health = startHealth;
		UpdateUI ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(float damage){
		health -= damage;



		var died = CheckDie ();

		if (!died) {
			DamageAnimation ();
		}
		UpdateUI ();
		Debug.Log ("take damage. health: " + health + ", damage: " + damage);
	}

	private void DamageAnimation(){
		animator.SetBool ("Damaged",true);

		Vector3 forceVec = rb.velocity.normalized * explosionStrength;
		rb.AddForce(forceVec,ForceMode.Acceleration);
	}

	private bool CheckDie(){
		if (health < 0) {
			health = 0.0f;
			Die ();
			return true;
		}
		return false;
	}

	private void HookHealth(float newHealth){
		health = newHealth;
		CheckDie ();
		UpdateUI();
	}

	private void UpdateUI(){
		float value = health / startHealth;
		healthSlider.value = value;
		if (isLocalPlayer) {
			Slider healthUISlider  = GameObject.FindGameObjectWithTag ("HealthSlider").GetComponent<Slider> ();
			healthUISlider.value = value;
		}
	}

	public void Die(){
		//TODO: respawn
	}

	void OnTriggerEnter(Collider other) {
		int layer = other.gameObject.layer;

		if (LayerMask.LayerToName(layer)== "Damage") {
			Fireball fireball = other.gameObject.GetComponent<Fireball> ();

			if (fireball != null) {
				TakeDamage (fireball.Force);
			}
		}
	}
}
