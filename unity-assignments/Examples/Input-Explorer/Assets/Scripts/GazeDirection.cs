using UnityEngine;
using System.Collections;

// IMPORTANT: The gaze direction data reported by the Gear VR is
// more accurate than the simple gyroscope data due to sensor fusion
// with the accelerometer data.

public class GazeDirection : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		
		// Set the GameObject's orientation based on the Gear VR's gaze vector
		gameObject.transform.localRotation = GameObject.Find("Main Camera").transform.localRotation;
		
	}
}
