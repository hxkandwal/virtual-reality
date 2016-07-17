using UnityEngine;
using System.Collections;

// IMPORTANT: The gyroscope must be enabled before it will report values.

public class Gyroscope : MonoBehaviour {

	// A correction offset must be used to align the gyroscope data
	Quaternion CorrectionOffset;
	
	// Use this for initialization
	void Start () {      
	
		// Determine if the system has a gyroscope
		if(SystemInfo.supportsGyroscope) {
			// Enable the gyroscope
			Input.gyro.enabled = true;
		}
		
		// Set the correction offset rotation
		CorrectionOffset = Quaternion.Euler(90.0f, 0.0f, 0.0f);
		
	}
	
	// Update is called once per frame
	void Update () {
	
		// Set the GameObject's orientation based on the correction offset and the current gyroscope data
		gameObject.transform.localRotation = CorrectionOffset * Input.gyro.attitude;
		
	}
}
