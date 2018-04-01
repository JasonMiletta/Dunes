using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region PARAMETERS
	public float speed = 90f;
    public float turnSpeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
	public float uprightTorque = 5f;
	#endregion

	#region STATE
    private float powerInput;
    private float turnInput;
    private Rigidbody rigidBody;
	#endregion

    void Awake () 
    {
        rigidBody = GetComponent <Rigidbody>();
    }

    void Update () 
    {
        powerInput = Input.GetAxis ("Vertical");
        turnInput = Input.GetAxis ("Horizontal");
    }

    void FixedUpdate()
    {   
		Ray ray = new Ray (transform.position, Vector3.down);
        RaycastHit hit;
		bool didHit = Physics.Raycast(ray, out hit, hoverHeight);

		rotationalCorrectionUpdate(didHit, hit);
		hoverUpdate(didHit, hit);

		//Forward Acceleration
        rigidBody.AddRelativeForce(0f, 0f, powerInput * speed);

		//Rotation Acceleration
        rigidBody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);

    }

	private void hoverUpdate(bool didHit, RaycastHit hit){
		if(didHit){
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            rigidBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
	}

	private void rotationalCorrectionUpdate(bool didHit, RaycastHit hit){
		Vector3 floorNormal = Vector3.up;
		if(didHit){
			floorNormal = hit.normal;
		}
		var rot = Quaternion.FromToRotation(transform.up, floorNormal);
 		rigidBody.AddTorque(new Vector3(rot.x, rot.y, rot.z)*uprightTorque);
	}
	
}
