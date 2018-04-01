using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	#region COMPONENTS
	public GameObject trackedObject;
	#endregion

	#region PARAMETERS
	public float smoothTime = 0.3f;
	#endregion

	#region STATE
	private Vector3 velocity = Vector3.zero;
	#endregion
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 targetPosition = (trackedObject.transform.localPosition - new Vector3(0f, -1.0f, 4.0f));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		//Quaternion lookRotation = Quaternion.LookRotation(trackedObject.transform.position - transform.position, transform.up);
		//transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, smoothTime);
		//transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}
}
