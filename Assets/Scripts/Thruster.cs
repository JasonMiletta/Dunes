using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour {

	#region COMPONENTS
	public Rigidbody ParentRigidbody;
	#endregion

	#region PARAMETERS
	public float hoverHeight = 1.5f;
	public float hoverForce = 20f;
	#endregion

    #region GIZMOS
    List<Ray> raysToDraw = new List<Ray>();
    #endregion
    void OnDrawGizmos(){
        foreach(Ray r in raysToDraw){
            Gizmos.DrawRay(r);
        }
    }

	// Use this for initialization
	void Start () {
		if(ParentRigidbody == null){
			ParentRigidbody = GetComponentInParent<Rigidbody>();
		}
		if(ParentRigidbody == null){
			Debug.LogError("Thruster.cs No rigidbody was assigned or found in the parent object of this thruster!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		thrusterHoverUpdate();
	}

    private void thrusterHoverUpdate(){
		Ray ray = new Ray (transform.position, Vector3.down);
		RaycastHit hit;
		bool didHit = Physics.Raycast(ray, out hit, hoverHeight);

		hoverUpdate(didHit, hit, transform.position);
	
    }
	
	private void hoverUpdate(bool didHit, RaycastHit hit, Vector3 thrusterPosition){
		if(didHit){
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            ParentRigidbody.AddForceAtPosition(appliedHoverForce, thrusterPosition, ForceMode.Acceleration);
            raysToDraw.Add(new Ray(thrusterPosition, appliedHoverForce));
		}
	}
}
