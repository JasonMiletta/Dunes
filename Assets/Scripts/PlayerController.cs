using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region PARAMETERS
	public float speed = 90f;
    public float turnSpeed = 5f;
	public float uprightTorque = 5f;
    public float boostPower = 2.0f;
	#endregion

	#region STATE
    private float powerInput;
    private float turnInput;
    private Rigidbody rigidBody;
	#endregion

    void OnDrawGizmos(){
    }

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
		bool didHit = Physics.Raycast(ray, out hit);

		rotationalCorrectionUpdate(didHit, hit);
        movementUpdate();

    }
    private void movementUpdate(){
        float boostMultiplier = 1.0f;
        if(Input.GetAxis("Sprint") > 0){
            boostMultiplier = boostPower;
        }
		//Forward Acceleration
        rigidBody.AddRelativeForce(0f, 0f, powerInput * speed * boostMultiplier);

		//Rotation Acceleration
        rigidBody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);
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
