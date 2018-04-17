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
	public const float cameraResetTime = 5.0f;
	#endregion

	#region STATE
	private Vector3 velocity = Vector3.zero;
	private float timeSinceLastCameraMovement = 0f;
	#endregion
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(timeSinceLastCameraMovement <= 0){
			Vector3 targetPosition = (trackedObject.transform.position + offsetVector);
			
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		} else {
			timeSinceLastCameraMovement -= Time.deltaTime;
		}

		transform.LookAt(trackedObject.transform, Vector3.up);
		
		lookUpdate();
	}

	void lookUpdate(){
		float horizontal = Input.GetAxis("HorizontalCamera");
		if(horizontal != 0) {
			transform.RotateAround(trackedObject.transform.position, Vector3.up, horizontal);
			timeSinceLastCameraMovement = cameraResetTime;
		} else{
			float mouseX = Input.GetAxis("Mouse X");
			if(mouseX != 0){
				transform.RotateAround(trackedObject.transform.position, Vector3.up, mouseX);
				timeSinceLastCameraMovement = cameraResetTime;
			}
		}
	}
}
