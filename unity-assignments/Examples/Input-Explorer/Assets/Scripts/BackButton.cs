using UnityEngine;
using System.Collections;

// The Back Button is located above the Touchpad with a circular arrow pointing backward.
// It acts as a mouse button at index 1.

// IMPORTANT: Once clicked, the Back Button does not stop reporting being pressed until
// the Touchpad is touched. Hence, you cannot use the duration of a Back Button press or
// consecutive Back Button presses as viable user input. Instead, you must require the 
// user to press the Touchpad after touching the Back Button.

public class BackButton : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
		// Check if the Back Button has been pressed
		if(Input.GetMouseButton(1)) {
			// Paint gameObject this script is attached to red
			gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
		// Back Button has not been pressed or the Touchpad has been touched since
		else {
			// Paint gameObject white
			gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	
	}
	
}
