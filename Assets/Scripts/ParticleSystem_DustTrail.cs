using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem_DustTrail : MonoBehaviour {

	#region COMPONENTS
	
	public ParticleSystem ParticleSystem;
	#endregion
	
	#region PARAMETERS
	public float emissionRatePerSpeedFactor = 1.0f;
	#endregion
	// Use this for initialization
	void Start () {
		if(ParticleSystem == null){
			Debug.LogWarning("ParticleSystem_DustTrail is missing its particleSystem");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Grab the position of the closest surface
		Vector3? ground = getClosestSurfacePosition();

		if(ground != null){
			//If we found a surface, place the particleSystem at that point
			Vector3 groundPosition = (Vector3)ground;
			ParticleSystem.transform.position = groundPosition;

			//Increase the emission rate based on the size of the parent object
			Rigidbody parentRigidBody = GetComponentInParent<Rigidbody>();
			if(parentRigidBody != null){
				float speedMagnitude = parentRigidBody.velocity.magnitude;
				Debug.Log(speedMagnitude);
				var emission = ParticleSystem.emission;
				emission.rateOverTime = speedMagnitude * emissionRatePerSpeedFactor;
			}
		}
	}


	private Vector3? getClosestSurfacePosition(){
		Ray ray = new Ray (transform.position, Vector3.down);
        RaycastHit hit;
		bool didHit = Physics.Raycast(ray, out hit);
		if(didHit){
			return hit.point;
		}
		return null;
	}
}
