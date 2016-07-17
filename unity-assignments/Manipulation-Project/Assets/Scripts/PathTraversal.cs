using UnityEngine;
using System.Collections;


// To play within unity, handle line numbers : 211, 285, 402

public class PathTraversal : MonoBehaviour
{
	// Public variables that are customizable in the Inspector
	public float rayRadius = 0.2f;
	public float rayMaxLength = 200.0f;
	public float speed = 0.1F;
	public GameObject metaObject;

	private GameObject lastSurvivingPathGameObject;
	private ArrayList glowingPathList = new ArrayList ();
	private bool isInManipulationZone = false;

	private GameObject handGameObject;
	private GameObject handHeldRay;
	private GameObject raycastObject;
	private GameObject mainCamera;
	private Transform grabbedObject = null;
	private GameObject user;
	private Transform manipulationAreaTransform;

	private bool isGrabbing = false;
	private bool manipulating = false;
	private bool hasPlayedOnce = false;
	private int hasPlayedOnceCount = 0;

	private GameObject armGrabbedObject;
	private ArrayList armGrabbedObjectParents = new ArrayList();

	public void setIsInManipulationZone(bool value)
	{
		isInManipulationZone = value;
	}

	void Start ()
	{
		OVRTouchpad.Create();
		OVRTouchpad.TouchHandler += HandleTouchHandler;

		glowingPathList.Add (metaObject);

		grabbedObject = GameObject.Find("Grabbed Object").transform;
		handGameObject = GameObject.Find ("Hand");
		mainCamera = GameObject.Find ("Main Camera");
		armGrabbedObject = GameObject.Find ("Arm Grabbed Object");
		handHeldRay = GameObject.Find ("Ray");

		handHeldRay.SetActive (false);
		user = handGameObject.transform.parent.gameObject;
	}

	void HandleTouchHandler (object sender, System.EventArgs e)
	{
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs) e;

		// locate arm leaf game object
		GameObject armLeafGameObject = armGrabbedObject;
		
		for ( int i = 0; i < armGrabbedObjectParents.Count; i++)
			armLeafGameObject = armLeafGameObject.transform.GetChild(0).gameObject;

		// If the touchpad is being pressed and an object is being intersected and it is manipulable
		if(!isGrabbing && touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap
			  && raycastObject != null && raycastObject.tag == "Manipulable") {
			
			// Change our state to grabbing
			isGrabbing = true;
			
			// Move the object under the empty grabbed object within the hierarchy to keep relative placement
			if (armGrabbedObjectParents.Count != 0) {
				raycastObject.transform.parent = grabbedObject.transform;
			} else {
				Debug.Log ("setting parent - "  + raycastObject.transform.parent.name);
				//armGrabbedObjectParents.Add(raycastObject.transform.parent.gameObject);
				//raycastObject.transform.parent = armLeafGameObject.transform;
				raycastObject.transform.parent = grabbedObject.transform;
				handGameObject.transform.LookAt(raycastObject.transform.position);
			}

			// Turn off the object's kinematics to avoid the object from spinning in place due to collisions
			raycastObject.GetComponent<Rigidbody>().isKinematic = true;
			
			// Color the ray yellow to indicate that an object is grabbed
			handHeldRay.GetComponent<Renderer>().material.color = Color.yellow;
		}

		// logic for ARM

		if(!isGrabbing && touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap 
		   && raycastObject != null && raycastObject.tag == "ARM Manipulable") {

			if (armGrabbedObjectParents.Count == 0) {
				armLeafGameObject.transform.position = raycastObject.transform.position;
			}
		
			// store users raycast object parent information
			Debug.Log ("setting parent - "  + raycastObject.transform.parent.name);
			armGrabbedObjectParents.Add(raycastObject.transform.parent.gameObject);

			raycastObject.transform.parent = armLeafGameObject.transform;

			//update main camera and hand position
			user.transform.position = new Vector3 (raycastObject.transform.position.x, 7f, raycastObject.transform.position.z);
			mainCamera.transform.rotation = user.transform.rotation;

			handGameObject.transform.position = mainCamera.transform.position + new Vector3 (0, -3, 0);
			handGameObject.transform.rotation = mainCamera.transform.rotation;
			handHeldRay.transform.rotation = handGameObject.transform.rotation;
			handHeldRay.transform.Rotate (new Vector3(90, 0, 0));

			// Turn off the object's kinematics to avoid the object from spinning in place due to collisions
			raycastObject.GetComponent<Rigidbody>().isKinematic = true;
			raycastObject.gameObject.SetActive (false); // so that we do not select it again
			
			// Color the ray yellow to indicate that an object is grabbed
			handHeldRay.GetComponent<Renderer>().material.color = Color.magenta;
		}

		if (!isGrabbing && touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap  && raycastObject != null &&
		    raycastObject.tag != "ARM Manipulable" && 
		    raycastObject.tag != "Manipulable" && 
		    (armGrabbedObjectParents.Count != 0)) {
			
			armLeafGameObject = armGrabbedObject;
			
			Debug.Log (" [Release] parents count (old) : " + armGrabbedObjectParents.Count);
			
			for ( int i = 0; i < armGrabbedObjectParents.Count; i++)
				armLeafGameObject = armLeafGameObject.transform.GetChild(0).gameObject;
			
			Debug.Log (" [Release] leaf object : " + armLeafGameObject.name);
			GameObject leafLevelParent = (GameObject) armGrabbedObjectParents [armGrabbedObjectParents.Count -1];
			
			Transform previousArmElement = armLeafGameObject.transform.parent;
			
			if (previousArmElement.name == "Arm Grabbed Object") {
				previousArmElement = lastSurvivingPathGameObject.transform;
				user.transform.position += new Vector3 (0, 0, -9);
			}
			
			Debug.Log (" Leaf parent name : " + leafLevelParent.transform.name);
			armLeafGameObject.transform.SetParent(leafLevelParent.transform);
			armGrabbedObjectParents.Remove(leafLevelParent);
			Debug.Log (" [Release] parents count (new) : " + armGrabbedObjectParents.Count);
			
			user.transform.position = new Vector3 (previousArmElement.transform.position.x, 7f, previousArmElement.transform.position.z);
			mainCamera.transform.rotation = user.transform.rotation;
			
			handGameObject.transform.position = mainCamera.transform.position + new Vector3 (0, -3, 0);
			handGameObject.transform.rotation = mainCamera.transform.rotation;
			handHeldRay.transform.rotation = handGameObject.transform.rotation;
			handHeldRay.transform.Rotate (new Vector3(90, 0, 0));
			
			armLeafGameObject.SetActive(true);
			armLeafGameObject.GetComponent<Rigidbody>().isKinematic = false;
			
			handHeldRay.GetComponent<Renderer>().material.color = Color.green;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isInManipulationZone) {

			// for input from button action only
			if (Input.GetMouseButton (1)) {
				ArrayList survivingPathList = new ArrayList ();

				for (int i = 0; i < glowingPathList.Count; i ++) {
					if (i > glowingPathList.Count - 2) {
						survivingPathList.Add (glowingPathList [i]);
						continue;
					}

					GameObject pathGameObject = (GameObject)glowingPathList [i];
			
					// Move the Navigation GameObject based on the travel vector
					transform.position = Vector3.Lerp (((GameObject)glowingPathList [i]).transform.position, 
				                                   ((GameObject)glowingPathList [i + 1]).transform.position, 0.5f * Time.deltaTime)
						+ new Vector3 (0, 5, -9);

					Destroy (pathGameObject);
				}
				glowingPathList.Clear ();
				glowingPathList = survivingPathList;
				lastSurvivingPathGameObject = (GameObject)glowingPathList [glowingPathList.Count - 1];

				if (isCollidedWithManipulationZone (lastSurvivingPathGameObject.transform.position, lastSurvivingPathGameObject.transform.forward, out manipulationAreaTransform)) {
					isInManipulationZone = true;

					lastSurvivingPathGameObject.name = "Last Samurai";
					lastSurvivingPathGameObject.SetActive (false);

					user.transform.position = lastSurvivingPathGameObject.transform.position + new Vector3 (0, 5, 0);
					handHeldRay.SetActive (true);
					handHeldRay.GetComponent<Renderer> ().material.color = Color.blue;

					manipulationAreaTransform.GetComponent<Collider>().enabled = false;
					Debug.Log (" Arrived Manipulation Zone");
				}
			}
		
			// for input from swipe action only
			else if (Input.GetMouseButton (0)) {
					GameObject glowingClone = null;
					Vector3 movedPosition = new Vector3 (Input.GetAxis ("Mouse X"), 0, Input.GetAxis ("Mouse Y"));
					movedPosition = new Vector3 (movedPosition.x, 0, movedPosition.z);
				    //movedPosition = new Vector3(-movedPosition.z, 0, -movedPosition.x);

					GameObject lastDetectedGameObject = (GameObject) glowingPathList [glowingPathList.Count - 1];
					Vector3 lastDetectedGameObjectPosition = lastDetectedGameObject.transform.position;
			
					Vector3 newGlowingClonePosition = lastDetectedGameObjectPosition + movedPosition;

					double lastNotedDistance = (newGlowingClonePosition - lastDetectedGameObjectPosition).magnitude;

					if (lastNotedDistance > 0.5f && !isCollided (lastDetectedGameObjectPosition, movedPosition)) {	
						Debug.Log (" x - " + newGlowingClonePosition.x + " , y - " + newGlowingClonePosition.y + " , z - " + newGlowingClonePosition.z);
					
						glowingClone = Instantiate (lastDetectedGameObject, newGlowingClonePosition, lastDetectedGameObject.transform.rotation) as GameObject;
				
						glowingClone.name = "glowing-" + glowingPathList.Count;
						glowingPathList.Add (glowingClone);
					}
			}
		} else {
			Debug.Log(" In manipulation Zone !!!");

			if(!isGrabbing) {
				// Update the ray based on the gaze direction
				UpdateRay();

				CheckExit();
			}
			// If an object is being grabbed
			else {
				GameObject armLeafGameObject = armGrabbedObject;

				if (armGrabbedObjectParents.Count != 0)
				{
					for ( int i = 0; i < armGrabbedObjectParents.Count; i++)
						armLeafGameObject = armLeafGameObject.transform.GetChild(0).gameObject;

					armLeafGameObject.transform.rotation = mainCamera.transform.rotation;
				}

				// Manipulate the object and ray based on the user's input
				ManipulateObject();

				// Then check if the user is releasing the object with a back button press
				CheckRelease();
			}
		}
	}

	// logic to check the exit from manipulation zone
	void CheckExit() {
		if (Input.GetMouseButton (1) && hasPlayedOnce && armGrabbedObjectParents.Count == 0) {
			Debug.Log (" Checking for EXIT consitions !!");

			isInManipulationZone = false;
			hasPlayedOnce = false;
		
			lastSurvivingPathGameObject.SetActive (true);
			
			user.transform.position = lastSurvivingPathGameObject.transform.position + new Vector3 (0, 5, -9);
			mainCamera.transform.position = user.transform.position;

			handHeldRay.SetActive (false);
			handHeldRay.GetComponent<Renderer> ().material.color = Color.blue;
			
			manipulationAreaTransform.GetComponent<Collider>().enabled = true;
			Debug.Log (" Departed from Manipulation Zone");
		}
	}

	// Update the ray based on the gaze direction
	void UpdateRay () {

		Transform cameraTransform = mainCamera.transform;
	    
		if (Input.GetMouseButton (0)) {
			cameraTransform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
		} 

		// Get the head position and gaze direction from the Main Camera object
		Vector3 headPosition = cameraTransform.position;
		Vector3 gazeDirection = cameraTransform.forward;
		
		// Use the hand position and ray objects to make technique appear like traditional raycasting
		Transform handPosition = handGameObject.transform;
		
		// Prepare to capture intersection data
		RaycastHit hitInfo;
		
		// Have the physics engine check for intersections with the vector originating at the head position
		// and heading in the direction of the user's gaze
		if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {

			Debug.Log (" Intersected element : " + hitInfo.transform.name);

			// Get the object being intersected
			raycastObject = hitInfo.transform.gameObject;

			// Move the relative grab point to the intersection point
			grabbedObject.position = hitInfo.transform.position;

			// Now update the ray
			// As the hand position object is holding the ray, we'll point it to the intersected object
			handPosition.LookAt(hitInfo.point);
			
			// Calculate the vector that will represent the ray and that vector's length
			Vector3 rayVector = hitInfo.point - handPosition.position;
			float rayLength = rayVector.magnitude;

			// Scale and position the ray object accordingly to the hand position
			//handHeldRay.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
			handHeldRay.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
			
			// Provide feedback whether the object can be manipulated
			// Only objects with the Manipulable tag can be manipulated
			if(raycastObject.tag == "Manipulable") {
				// Indicate manipulable objects with a green ray
				handHeldRay.GetComponent<Renderer>().material.color = Color.green;
			}
			else if(raycastObject.tag == "ARM Manipulable") {
				// Indicate manipulable objects with a green ray
				handHeldRay.GetComponent<Renderer>().material.color = Color.red;
			}
			// Not manipulable 
			else {
				// Indicate with a default ray color
				if (armGrabbedObjectParents.Count == 0)
					handHeldRay.GetComponent<Renderer>().material.color = Color.blue;
				else
					handHeldRay.GetComponent<Renderer>().material.color = Color.magenta;
			}
		}
		// No object was intersected by the user's gaze
		else {
			
			// Indicate a null object to ensure we don't try to fetch properties of it
			raycastObject = null;

			// Have the ray point in whatever direction the Main Camera is pointing
			handGameObject.transform.rotation = cameraTransform.rotation;
			handHeldRay.transform.rotation = handGameObject.transform.rotation;
			handHeldRay.transform.Rotate (new Vector3(90, 0, 0));

			// Have the ray extend out to the maximum length
			//handHeldRay.transform.localPosition = new Vector3(0.0f, 0.0f, rayMaxLength/2.0f);
			handHeldRay.transform.localScale = new Vector3(rayRadius, rayMaxLength/2.0f, rayRadius);
			
			// Indicate with a default ray color
			if (armGrabbedObjectParents.Count == 0)
				handHeldRay.GetComponent<Renderer>().material.color = Color.blue;
			else
				handHeldRay.GetComponent<Renderer>().material.color = Color.magenta;
		}
		
		if (hasPlayedOnceCount < 3) {
			hasPlayedOnceCount ++;
		} else {
			hasPlayedOnceCount = 0;
			hasPlayedOnce = true;
		}
	}

	public bool isCollided (Vector3 position, Vector3 direction)
	{
		bool result = false;
		if (Physics.Raycast (position, direction, 2.0f)) {
			result = true;
		}
		Debug.Log (" returning " + result);
		return result;
	}
	
	public bool isCollidedWithManipulationZone (Vector3 position, Vector3 direction, out Transform collidedWith)
	{
		bool result = false;
		collidedWith = null;
		RaycastHit raycastHitObject;

		if (Physics.Raycast (position, direction, out raycastHitObject,  2.0f)) {
			if (raycastHitObject.transform.tag == "Manipulation Zone") {
				result = true;
				collidedWith = raycastHitObject.transform;
			}
		}
		return result;
	}

	// Manipulate the object and ray based on the user's input
	void ManipulateObject () {
		
		// Get any forward or backward swipes from the touchpad
		float SwipeX = Input.GetAxis("Mouse Y");

		// locate arm leaf game object
		Transform transformToManipulate = grabbedObject.transform;

		float objectDistance = transformToManipulate.position.z;

		Debug.Log (" [manipulation] leaf object : " + transformToManipulate.name);

		// If there are any swipes and the touchpad was already pressed
		if(Mathf.Abs(SwipeX) != 0.0f && manipulating) {


			// Move the object forward or backward based on the swipe
			objectDistance -= SwipeX;
			
			// Keep the object a minimum distance of 1 meter away
			if(objectDistance < 2.0f) {
				objectDistance = 2.0f;
			}
		}

		Debug.Log ("[M] objectDistance : " + objectDistance);

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

		Debug.Log ("[M] grabbed object (old) : " + transformToManipulate.position.x + " , " + transformToManipulate.position.y + " , " + objectDistance);

		// Update the object's location based on the new object distance
		transformToManipulate.position = new Vector3(transformToManipulate.position.x, transformToManipulate.position.y , objectDistance);
		
		// Point the hand position to the grabbed object's location
		handGameObject.transform.LookAt(transformToManipulate.position);

		// Calculate the vector that will represent the ray and that vector's length
		Vector3 rayVector = transformToManipulate.position - handGameObject.transform.position;
		float rayLength = rayVector.magnitude;
		
		// Scale and position the ray object accordingly to the hand position
		//rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
		handHeldRay.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
	}
	
	// Check if the user is releasing the object with a back button press
	void CheckRelease () {
		
		// Back button is pressed and there is an object being manipulated

		if (Input.GetMouseButton (1) && grabbedObject.childCount != 0 && isGrabbing) {

			Debug.Log (" [Release] release grabbed objects child :  " + grabbedObject.transform.GetChild(0).transform.name);
		
			// Change our state back to not grabbing
			isGrabbing = false;

			// Put the object on the top level of the scene hierarchy
			Transform grabbedChild = grabbedObject.transform.GetChild(0).transform;
			grabbedChild.parent = GameObject.Find ("Manipulable Objects").transform;
			
			// Allow the physics engine to control the object
			grabbedChild.GetComponent<Rigidbody> ().isKinematic = false;
			
			// Keep track that we're no longer grabbing an object
			raycastObject = null;
			
			// Color the ray green since we're still pointing at a manipulable object.
			// This avoids color blinking.
			handHeldRay.GetComponent<Renderer> ().material.color = Color.green;
		}
	}
}
