using UnityEngine;
using System.Collections;

// Attach to camera to simulate HMD head movements using the arrow keys
public class GearSimulator : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
		// Calculate heading change based on the left and right arrows
		float heading = Input.GetAxis("Horizontal") * Time.deltaTime * 15.0f;
		
		// Calculate pitch change based on the up and down arrows
		float pitch = Input.GetAxis("Vertical") * Time.deltaTime * 15.0f;
		
		// Apply heading and pitch to camera
		transform.Rotate(-pitch, heading, 0.0f);
	}
}
