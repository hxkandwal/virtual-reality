using UnityEngine;
using System.Collections;

// The Touchpad is located on the right side of the Gear VR and is indented. 
// It acts as a mouse button at index 0 and as the Mouse X and Mouse Y axes.

// IMPORTANT: The X axes is flipped. A swipe forward yields a negative X value while
// a swipe backward yields a positive X value.

public class Touchpad : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
		// Check if the Touchpad is being touched
		if(Input.GetMouseButton(0)) {
			// Paint gameObject this script is attached to red
			gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
		// Touchpad is not being touched
		else {
			// Paint gameObject white
			gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	
		// Determine if there is any swipe motions
		float SwipeX = Input.GetAxis("Mouse X");
		float SwipeY = Input.GetAxis("Mouse Y");
		
		// Check if there is some swipe motion
		if(Mathf.Abs(SwipeX) + Mathf.Abs(SwipeY) != 0.0f) {
			// Move the gameObject along the Z axis with an X swipe and Y axis with a Y swipe
			gameObject.transform.localPosition = new Vector3(0.0f, SwipeY, -SwipeX);
		}
		// There is no swipe motion
		else {
			// Move the gameObject back to original position
			gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		}
	
	}
}
