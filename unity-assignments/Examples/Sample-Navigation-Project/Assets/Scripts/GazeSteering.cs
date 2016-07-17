using UnityEngine;
using System.Collections;

// Notes:
// Attach to any GameObject

public class GazeSteering : MonoBehaviour {

	public int travelButton = 0;
	public float travelSpeed = 1.0f;

	// Update is called once per frame
	void Update () {
	
		// Get the current gaze direction from the Main Camera 
		Quaternion gazeDirection = GameObject.Find("Main Camera").transform.localRotation;
		
		// If the travel button is pressed
		if(Input.GetMouseButton(travelButton)) {
			
			// Determine the travel vector based on the gaze direction and provided speed
			Vector3 travelVector = gazeDirection * Vector3.forward * travelSpeed;
			
			// Move the Navigation GameObject based on the travel vector
			GameObject.Find("Navigation").transform.Translate(travelVector);
		
		}
	}
}
