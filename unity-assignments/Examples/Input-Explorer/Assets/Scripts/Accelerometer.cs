using UnityEngine;
using System.Collections;

// IMPORTANT: Due to gravity, one axis of the accelerometer will always report
// negative values.

public class Accelerometer : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
		// Determine the current acceleration vector
		Vector3 AccelerationVector = Input.acceleration;
		
		// Flip the Z axis value		
		AccelerationVector.z = -AccelerationVector.z;
	
		// Move the GameObject based on the acceleration vector
		gameObject.transform.Translate(AccelerationVector);
		
		// Due to moving the GameObject every frame, we must keep the
		// object within a confined volume, or it will fly off into space.
		
		// Get the GameObject's new position
		Vector3 CurrentPosition = gameObject.transform.localPosition;
		
		// Keep it within 1.0 unit along the X axis
		if(CurrentPosition.x < -0.5f) { CurrentPosition.x = -0.5f; }
		else if(CurrentPosition.x > 0.5f) { CurrentPosition.x = 0.5f; }
		
		// Keep it within 1.0 unit along the Y axis
		if(CurrentPosition.y < -0.5f) { CurrentPosition.y = -0.5f; }
		else if(CurrentPosition.y > 0.5f) { CurrentPosition.y = 0.5f; }
		
		// Keep it within 1.0 unit along the Z axis
		if(CurrentPosition.z < -0.5f) { CurrentPosition.z = -0.5f; }
		else if(CurrentPosition.z > 0.5f) { CurrentPosition.z = 0.5f; }
		
		// Update the position based on the new boundaries
		gameObject.transform.localPosition = CurrentPosition;
	
	}
}
