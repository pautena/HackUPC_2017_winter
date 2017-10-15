using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyFireball : NetworkBehaviour {

	public float Force = 10;
	public float Radius = 10;
	public GameObject explotion;
	public LayerMask playerMask;
	public float explosionRadius=5f;
	public float explosionForce=1000f;
	public float maxDamage=100f;

	[ServerCallback]
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 2) {


			//create fireball explotion after collides
			RpcExplotion();
			ShowExplosion ();
			DoDamage ();
			NetworkServer.Destroy (gameObject);
		}
	}

	[ClientRpc]
	private void RpcExplotion(){
		ShowExplosion ();
	}

	private void ShowExplosion(){
		GameObject fireballexplotion = Instantiate(explotion);
		fireballexplotion.transform.position = transform.position;
		Destroy (fireballexplotion,2f);
	}

	private void DoDamage(){

		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, playerMask);

		// Go through all the colliders...
		for (int i = 0; i < colliders.Length; i++){
			Debug.Log ("collider. gameObject: "+colliders[i].gameObject);

			// ... and find their rigidbody.
			Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();

			// If they don't have a rigidbody, go on to the next collider.
			if (!targetRigidbody)
				continue;

			// Add an explosion force.
			targetRigidbody.AddExplosionForce (explosionForce, transform.position, explosionRadius);

			// Find the TankHealth script associated with the rigidbody.
			DragonHealth targetHealth = targetRigidbody.GetComponent<DragonHealth> ();

			// If there is no TankHealth script attached to the gameobject, go on to the next collider.
			if (!targetHealth)
				continue;

			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = CalculateDamage (targetRigidbody.position);

			// Deal this damage to the tank.
			targetHealth.TakeDamage (damage);
		}

	}


	private float CalculateDamage (Vector3 targetPosition){
		// Create a vector from the shell to the target.
		Vector3 explosionToTarget = targetPosition - transform.position;

		// Calculate the distance from the shell to the target.
		float explosionDistance = explosionToTarget.magnitude;

		// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
		float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

		// Calculate damage as this proportion of the maximum possible damage.
		float damage = relativeDistance * maxDamage;

		// Make sure that the minimum damage is always 0.
		damage = Mathf.Max (0f, damage);

		return damage;
	}
}
