using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyFireball : NetworkBehaviour {

	public float Force = 10;
	public float Radius = 10;
	public GameObject explotion;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 2) {
			CmdOnTriggerEnter ();
		}
	}

	[Command]
	void CmdOnTriggerEnter(){		
		/*Rigidbody impact = other.GetComponent<Rigidbody>();
		if (impact){
			impact.AddExplosionForce(100f * Force, transform.position, 100f * Radius);
		}*/


		//create fireball explotion after collides
		RpcExplotion();
		ShowExplosion ();
		NetworkServer.Destroy (gameObject);
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
}
