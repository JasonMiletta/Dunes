using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	#region COMPONENTS
	public GameObject trackedObject;
	private Camera camera;
	#endregion

	#region PARAMETERS
	public float smoothTime = 0.3f;
	public Vector3 offsetVector = new Vector3(0f, 2.0f, -8.0f);
	public const float cameraResetTime = 2.0f;
	public float rotationSmoothTime = 5.0f;
	public float lookSpeed = 100.0f;
	public float maxCameraFov = 90.0f;
	#endregion

	#region STATE
	private Vector3 velocity = Vector3.zero;
	private float timeSinceLastCameraMovement = 0f;
	private float currentLookAngle = 0f;
	private Vector3 targetPosition;
	private float originalCameraFov;
	private float currentCameraFov;
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
		camera = GetComponent<Camera>();
		if(camera == null){
			Debug.LogWarning("PlayerCamera can't find a Camera object!");
		} else {
			originalCameraFov = camera.fieldOfView;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		visualizeSpeed();
		Vector3 targetPosition = trackedObject.transform.position;

		//GET UP
		targetPosition = trackedObject.transform.position + new Vector3(0.0f, offsetVector.y, 0.0f);
		
	
		//get rotation of trackedObject
		Quaternion trackedObjectRotation = trackedObject.transform.rotation;
		Vector3 trackedObjectEulerAngles = trackedObjectRotation.eulerAngles;

		float rotationAngle = trackedObjectEulerAngles.y;
		
		lookUpdate();

		if(timeSinceLastCameraMovement <= 0){
			currentLookAngle = rotationAngle;
		
		} else {
			rotationAngle = currentLookAngle;
			timeSinceLastCameraMovement -= Time.deltaTime;
		}
	
		//GET BEHIND
		float yRotationInRadians = Mathf.Deg2Rad * (rotationAngle);
		float xCoord = Mathf.Sin(yRotationInRadians);
		float zCoord = Mathf.Cos(yRotationInRadians);
		targetPosition = targetPosition + new Vector3(xCoord * offsetVector.z, 0.0f, zCoord * offsetVector.z);

		//Smooth rotation to look at target
		Quaternion targetRotation = transform.rotation;
		targetRotation.eulerAngles = new Vector3(targetRotation.x, rotationAngle, targetRotation.z); 
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothTime);

		//Move Camera to targetPosition, behind the trackedObject
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		//transform.LookAt(trackedObject.transform, Vector3.up);
		
	}

	void lookUpdate(){
		float horizontal = Input.GetAxis("HorizontalCamera");
		if(horizontal != 0) {
			//transform.RotateAround(trackedObject.transform.position, Vector3.up, horizontal);
			currentLookAngle += horizontal * Time.deltaTime * lookSpeed;
			timeSinceLastCameraMovement = cameraResetTime;
		} else{
			float mouseX = Input.GetAxis("Mouse X");
			if(mouseX != 0){
				currentLookAngle += mouseX * Time.deltaTime * lookSpeed;
				//transform.RotateAround(trackedObject.transform.position, Vector3.up, mouseX);
				timeSinceLastCameraMovement = cameraResetTime;
			}
		}
	}

	void visualizeSpeed(){
		Rigidbody trackedObjectRigidBody = trackedObject.GetComponent<Rigidbody>();
		if(trackedObjectRigidBody != null){
			float speedMagnitude = trackedObjectRigidBody.velocity.magnitude;
			currentCameraFov = originalCameraFov + (speedMagnitude);
			if(currentCameraFov > maxCameraFov){
				currentCameraFov = maxCameraFov;
			}
			camera.fieldOfView = currentCameraFov;
		}
	}
}
