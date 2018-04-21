using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	#region COMPONENTS
	public GameObject trackedObject;
	#endregion

	#region PARAMETERS
	public float smoothTime = 0.3f;
	public Vector3 offsetVector = new Vector3(0f, 1.0f, -8.0f);
	public const float cameraResetTime = 5.0f;
	public float rotationSmoothTime = 5.0f;
	#endregion

	#region STATE
	private Vector3 velocity = Vector3.zero;
	private float timeSinceLastCameraMovement = 0f;
	private Vector3 targetPosition;
	#endregion


    #region GIZMOS
    List<Ray> raysToDraw = new List<Ray>();
    #endregion
    void OnDrawGizmos(){
        foreach(Ray r in raysToDraw){
            Gizmos.DrawRay(r);
        }
		raysToDraw = new List<Ray>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//lookUpdate();
		
		//get rotation of trackedObject
		Quaternion trackedObjectRotation = trackedObject.transform.rotation;
		Vector3 trackedObjectEulerAngles = trackedObjectRotation.eulerAngles;

		Vector3 targetPosition = trackedObject.transform.position;

		//GET UP
		targetPosition = trackedObject.transform.position + new Vector3(0.0f, offsetVector.y, 0.0f);
		Debug.DrawLine(trackedObject.transform.position, targetPosition, Color.red);
		
		//GET BEHIND
		float yRotationInRadians = Mathf.Deg2Rad * (trackedObjectEulerAngles.y);
		float xCoord = Mathf.Sin(yRotationInRadians);
		float zCoord = Mathf.Cos(yRotationInRadians);
		targetPosition = targetPosition + new Vector3(xCoord * offsetVector.z, 0.0f, zCoord * offsetVector.z);
		Debug.DrawLine(trackedObject.transform.position, targetPosition, Color.red);

		//Smooth rotation to look at target
		Quaternion targetRotation = transform.rotation;
		targetRotation.eulerAngles = new Vector3(targetRotation.x, trackedObjectEulerAngles.y, targetRotation.z); 
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothTime);

		//Move Camera to targetPosition, behind the trackedObject
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		//transform.LookAt(trackedObject.transform, Vector3.up);
		if(timeSinceLastCameraMovement <= 0){
		} else {
			timeSinceLastCameraMovement -= Time.deltaTime;
		}

		
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
