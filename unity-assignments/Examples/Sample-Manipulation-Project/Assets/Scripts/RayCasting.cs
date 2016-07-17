using UnityEngine;
using System.Collections;

// Notes:
// Attach to any GameObject

public class RayCasting : MonoBehaviour {

	// Public variables that are customizable in the Inspector
	public float rayRadius = 0.025f;
	public float rayMaxLength = 100.0f;
	
	// State variables and data required among the functions of the class
	bool grabbing = false;
	bool manipulating = false;
	float objectDistance = 0.0f;
	GameObject raycastObject = null;
	Transform grabbedObject = null;

	// Use this for initialization
	void Start () {
	
		// Get reference to empty GameObject located under the Main Camera.
		// This object serves as the grabbing point relative to the grabbed object.
		grabbedObject = GameObject.Find("Grabbed Object").transform;
	
		// Color the ray GameObject blue by default
		GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.blue;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// If an object is not being grabbed
		if(!grabbing) {
			// Update the ray based on the gaze direction
			UpdateRay();
			// Then check to see if the user is grabbing it with a touchpad press
			CheckGrab();
		}
		// If an object is being grabbed
		else {
			// Manipulate the object and ray based on the user's input
			ManipulateObject();
			// Then check if the user is releasing the object with a back button press
			CheckRelease();
		}
	
	}
	
	// Update the ray based on the gaze direction
	void UpdateRay () {
		
		// Get the head position and gaze direction from the Main Camera object
		Vector3 headPosition = GameObject.Find("Main Camera").transform.position;
		Vector3 gazeDirection = GameObject.Find("Main Camera").transform.forward;
		
		// Use the hand position and ray objects to make technique appear like traditional raycasting
		Transform handPosition = GameObject.Find("Hand Position").transform;
		GameObject rayObject = GameObject.Find("Ray");
		
		// Prepare to capture intersection data
		RaycastHit hitInfo;
		
		// Have the physics engine check for intersections with the vector originating at the head position
		// and heading in the direction of the user's gaze
		if(Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
		
			// Get the object being intersected
			raycastObject = hitInfo.transform.gameObject;
			
			// Move the relative grab point to the intersection point
			grabbedObject.transform.position = hitInfo.point;
			
			// Keep track of the original distance to the object
			objectDistance = (grabbedObject.transform.position - headPosition).magnitude;
			
			// Now update the ray
			
			// As the hand position object is holding the ray, we'll point it to the intersected object
			handPosition.LookAt(hitInfo.point);
			
			// Calculate the vector that will represent the ray and that vector's length
			Vector3 rayVector = hitInfo.point - handPosition.position;
			float rayLength = rayVector.magnitude;
			
			// Scale and position the ray object accordingly to the hand position
			rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
			rayObject.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
			
			// Provide feedback whether the object can be manipulated
			// Only objects with the Manipulable tag can be manipulated
			if(raycastObject.tag == "Manipulable") {
				// Indicate manipulable objects with a green ray
				rayObject.GetComponent<Renderer>().material.color = Color.green;
			}
			// Not manipulable 
			else {
				// Indicate with a default blue ray
				rayObject.GetComponent<Renderer>().material.color = Color.blue;
			}
		}
		// No object was intersected by the user's gaze
		else {
		
			// Indicate a null object to ensure we don't try to fetch properties of it
			raycastObject = null;
			
			// Have the ray point in whatever direction the Main Camera is pointing
			handPosition.localRotation = GameObject.Find("Main Camera").transform.rotation;
			
			// Have the ray extend out to the maximum length
			rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayMaxLength/2.0f);
			rayObject.transform.localScale = new Vector3(rayRadius, rayMaxLength/2.0f, rayRadius);
			
			// Show the default blue ray
			rayObject.GetComponent<Renderer>().material.color = Color.blue;
		}
	}
	
	// Check to see if the user is grabbing the object with a touchpad press
	void CheckGrab () {
		
		// If the touchpad is being pressed and an object is being intersected and it is manipulable
		if(Input.GetMouseButton(0) && raycastObject != null && raycastObject.tag == "Manipulable") {
			
			// Change our state to grabbing
			grabbing = true;
			
			// Move the object under the empty grabbed object within the hierarchy to keep relative placement
			raycastObject.transform.parent = grabbedObject;
			
			// Turn off the object's kinematics to avoid the object from spinning in place due to collisions
			raycastObject.GetComponent<Rigidbody>().isKinematic = true;
			
			// Color the ray yellow to indicate that an object is grabbed
			GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.yellow;
		}
	}
	
	// Manipulate the object and ray based on the user's input
	void ManipulateObject () {
	
		// Get any forward or backward swipes from the touchpad
		float SwipeX = Input.GetAxis("Mouse X");
		
		// If there are any swipes and the touchpad was already pressed
		if(Mathf.Abs(SwipeX) != 0.0f && manipulating) {
			
			// Move the object forward or backward based on the swipe
			objectDistance -= SwipeX;
			
			// Keep the object a minimum distance of 1 meter away
			if(objectDistance < 1.0f) {
				objectDistance = 1.0f;
			}
		}
		
		// IMPORTANT: Swipes are based on the last touched position and the newest touched position.
		// Hence, if the user stopes touching the touchpad and then touches it again, a swipe from the
		// last touched point to the new touched point is generated. This will counteract any previous
		// input based on swipes, if you're not careful to check for this situation.
		
		// Check if the touchpad is being pressed
		if(Input.GetMouseButton(0)) {
			// Then we can accept a swipe next frame
			manipulating = true;
		}
		// If the touchpad is not being pressed
		else {
			// We will not accept a swipe next frame
			manipulating = false;
		}
		
		// Update the object's location based on the new object distance
		Vector3 objectLocation = grabbedObject.localPosition;
		objectLocation.z = objectDistance;
		grabbedObject.localPosition = objectLocation;
		
		// Use the hand position and ray objects to make technique appear like traditional raycasting
		Transform handPosition = GameObject.Find("Hand Position").transform;
		GameObject rayObject = GameObject.Find("Ray");
		
		// Point the hand position to the grabbed object's location
		handPosition.LookAt(grabbedObject.position);
		
		// Calculate the vector that will represent the ray and that vector's length
		Vector3 rayVector = grabbedObject.position - handPosition.position;
		float rayLength = rayVector.magnitude;
		
		// Scale and position the ray object accordingly to the hand position
		rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
		rayObject.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
	}
	
	// Check if the user is releasing the object with a back button press
	void CheckRelease () {
		
		// Back button is pressed and there is an object being manipulated
		if(Input.GetMouseButton(1) && raycastObject != null) {
			
			// Change our state back to not grabbing
			grabbing = false;
			
			// Put the object on the top level of the scene hierarchy
			raycastObject.transform.parent = null;
			
			// Allow the physics engine to control the object
			raycastObject.GetComponent<Rigidbody>().isKinematic = false;
			
			// Keep track that we're no longer grabbing an object
			raycastObject = null;
			
			// Color the ray green since we're still pointing at a manipulable object.
			// This avoids color blinking.
			GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.green;
		}
	}
}
