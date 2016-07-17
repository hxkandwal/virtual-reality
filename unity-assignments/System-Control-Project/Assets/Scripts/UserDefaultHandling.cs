using UnityEngine;
using System.Collections;

public class UserDefaultHandling : MonoBehaviour
{

	public float speed = 0.1F;
	public float rayRadius = 0.2f;
	public float rayMaxLength = 200.0f;
	public bool isInSystemControl;
	public bool isIntroduced;

	private GameObject hand;
	private GameObject ray;
	private GameObject mainCamera;
	private GameObject raycastObject;
	private GameObject welcomeMenu;
	private GameObject exitMenu;
	private GameObject environmentControlMenu;
	private GameObject objectControlMenu;
	private GameObject objectRotateControlMenu;
	private GameObject objectTranslateControlMenu;
	private GameObject cube;

	// Use this for initialization
	void Start ()
	{
		hand = GameObject.Find ("Hand");
		ray = GameObject.Find ("Ray");
		mainCamera = GameObject.Find ("Main Camera");
		welcomeMenu = GameObject.Find ("WelcomeMenu");
		exitMenu = GameObject.Find ("ExitConfirmationMenu");
		environmentControlMenu = GameObject.Find ("EnvironmentControlMenu");
		objectControlMenu = GameObject.Find ("ObjectControlMenu");
		objectRotateControlMenu = GameObject.Find ("ObjectRotateControlMenu");
		objectTranslateControlMenu = GameObject.Find ("ObjectTranslateControlMenu");
		cube = GameObject.Find ("Cube");

		welcomeMenu.SetActive (true);
		exitMenu.SetActive (false);
		environmentControlMenu.SetActive (false);
		objectControlMenu.SetActive (false);
		objectTranslateControlMenu.SetActive (false);
		objectRotateControlMenu.SetActive (false);
		cube.SetActive (false);

		OVRTouchpad.Create ();
		OVRTouchpad.TouchHandler += HandleTouchHandler;
	}

	void HandleTouchHandler (object sender, System.EventArgs e)
	{
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)e;
		
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap && raycastObject != null 
			&& raycastObject.tag == "WelcomeMenu") {
			if (raycastObject.name == "ExitText") {
				Debug.Log ("Single tap exit text");
				welcomeMenu.SetActive (false);
				exitMenu.SetActive (true);

			}
			if (raycastObject.name == "NoExit") {
				Debug.Log ("Single tap no exit text");
				welcomeMenu.SetActive (true);
				exitMenu.SetActive (false);

			}
			if (raycastObject.name == "YesExit") {
				Debug.Log ("Single tap yes exit text");
				Application.Quit ();
			}
			if (raycastObject.name == "EnterText") {
				Debug.Log ("Single tap enter text");
				welcomeMenu.SetActive (false);
				exitMenu.SetActive (false);
				cube.SetActive (true);
				isIntroduced = true;
			}
		}

		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap && raycastObject != null 
		    && raycastObject.tag == "ObjectControl") {

			if (raycastObject.name == "Rotate") {
				Debug.Log ("Single tap rotate");
				objectControlMenu.SetActive (false);
				objectRotateControlMenu.SetActive (true);
			}
			if (raycastObject.name == "Translate") {
				Debug.Log ("Single tap translate");
				objectControlMenu.SetActive (false);
				objectTranslateControlMenu.SetActive (true);
			}
			if (raycastObject.name == "Destroy") {
				Debug.Log ("Single tap destroy");
				cube.SetActive(false);
				objectControlMenu.SetActive(false);
			}
		}

		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap && raycastObject != null 
		    && raycastObject.tag == "ObjectTranslateControl") {
			
			if (raycastObject.name == "TranslateX") {
				Debug.Log ("Single tap TranslateX");
				cube.transform.position += new Vector3(5, 0, 0);
			}
			if (raycastObject.name == "TranslateY") {
				Debug.Log ("Single tap TranslateY");
				cube.transform.position += new Vector3(0, 5, 0);
			}
			if (raycastObject.name == "TranslateZ") {
				Debug.Log ("Single tap TranslateZ");
				cube.transform.position += new Vector3(0, 0, 5);
			}
			if (raycastObject.name == "GoBack") {
				Debug.Log ("Single tap Back");
				objectTranslateControlMenu.SetActive(false);
				objectControlMenu.SetActive(true);
			}
		}

		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap && raycastObject != null 
		    && raycastObject.tag == "ObjectRotateControl") {
			
			if (raycastObject.name == "RotateX") {
				Debug.Log ("Single tap RotateX");
				cube.transform.Rotate (30, 0, 0);
			}
			if (raycastObject.name == "RotateY") {
				Debug.Log ("Single tap RotateY");
				cube.transform.Rotate (0, 30, 0);
			}
			if (raycastObject.name == "RotateZ") {
				Debug.Log ("Single tap RotateZ");
				cube.transform.Rotate (0, 0, 30);
			}
			if (raycastObject.name == "GoBack") {
				Debug.Log ("Single tap Back");
				objectRotateControlMenu.SetActive(false);
				objectControlMenu.SetActive(true);
			}
		}

		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap && raycastObject != null && raycastObject.tag == "Manipulable") {

			// Color the ray yellow to indicate that an object is grabbed
			ray.GetComponent<Renderer> ().material.color = Color.yellow;
			
			objectControlMenu.SetActive (true);
		}

	}
	// Update is called once per frame
	void Update ()
	{
		// Update the ray based on the gaze direction
		UpdateRay ();

		// Then check if the user is releasing the object with a back button press
		CheckRelease ();
	}

	// Update the ray based on the gaze direction
	void UpdateRay ()
	{

		if (Input.GetMouseButton (0)) {
			mainCamera.transform.Rotate (Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), 0);
		} 

		// Get the head position and gaze direction from the Main Camera object
		Vector3 headPosition = mainCamera.transform.position;
		Vector3 gazeDirection = mainCamera.transform.forward;
		
		// Use the hand position and ray objects to make technique appear like traditional raycasting
		Transform handPosition = hand.transform;

		// Prepare to capture intersection data
		RaycastHit hitInfo;
		
		// Have the physics engine check for intersections with the vector originating at the head position
		// and heading in the direction of the user's gaze
		if ((Physics.Raycast (headPosition, gazeDirection, out hitInfo))) {

			// Get the object being intersected
			raycastObject = hitInfo.transform.gameObject;

			Debug.Log (" Intersected element : " + hitInfo.transform.name);

			// As the hand position object is holding the ray, we'll point it to the intersected object
			handPosition.LookAt (hitInfo.point);
			
			// Calculate the vector that will represent the ray and that vector's length
			Vector3 rayVector = hitInfo.point - handPosition.position;
			float rayLength = rayVector.magnitude;

			ray.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
			
			// Provide feedback whether the object can be manipulated
			// Only objects with the Manipulable tag can be manipulated
			if (raycastObject.tag == "Manipulable" || raycastObject.tag == "ObjectControl"
			    || raycastObject.tag == "ObjectTranslateControl" || raycastObject.tag == "ObjectRotateControl") {
				// Indicate manipulable objects with a green ray
				ray.GetComponent<Renderer> ().material.color = Color.green;
			}
			else if (raycastObject.tag == "WelcomeMenu") {
				// Indicate manipulable objects with a green ray
				ray.GetComponent<Renderer> ().material.color = Color.magenta;
			}
			// Not manipulable 
			else {
				// Indicate with a default blue ray
				ray.GetComponent<Renderer> ().material.color = Color.blue;
			}

		}
		// No object was intersected by the user's gaze
		else {
			Debug.Log ("No Hit area !");

			// Indicate a null object to ensure we don't try to fetch properties of it
			raycastObject = null;

			// Have the ray point in whatever direction the Main Camera is pointing
			hand.transform.rotation = mainCamera.transform.rotation;

			// Show the default blue ray

			ray.GetComponent<Renderer> ().material.color = Color.blue;
		}
	}

	// Check if the user is releasing the object with a back button press
	void CheckRelease ()
	{
		if (isIntroduced && Input.GetMouseButton(1)) {
			welcomeMenu.SetActive (false);
			exitMenu.SetActive (false);
			environmentControlMenu.SetActive (false);
			objectControlMenu.SetActive (false);
			objectTranslateControlMenu.SetActive (false);
			objectRotateControlMenu.SetActive (false);
		}
	}
}
