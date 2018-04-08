using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	#region COMPONENTS
	public GameObject trackedObject;
	#endregion

	#region PARAMETERS
	public float smoothTime = 0.3f;
	public Vector3 offsetVector = new Vector3(0f, -1.0f, 4.0f);
	#endregion

	#region STATE
	private Vector3 velocity = Vector3.zero;
	#endregion
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 targetPosition = (trackedObject.transform.localPosition + offsetVector);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

		transform.LookAt(trackedObject.transform, Vector3.up);
	}
}
