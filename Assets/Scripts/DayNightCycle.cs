using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	#region COMPONENTS
	public GameObject Sun;
	#endregion

	#region PARAMETERS
	public float dayTimeSpeed = 1.0f;
	#endregion

	#region VARIABLES
	private float tickAngle;
	#endregion

	// Use this for initialization
	void Start () {
		tickAngle = dayTimeSpeed / 360f;
	}
	
	// Update is called once per frame
	void Update () {
		DayNightCycleUpdate();
		
	}

	void DayNightCycleUpdate(){
        this.transform.Rotate(new Vector3(0, 1), tickAngle);
	}
}
