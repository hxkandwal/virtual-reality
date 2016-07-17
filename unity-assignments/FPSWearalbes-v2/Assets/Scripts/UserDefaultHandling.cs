using UnityEngine;
using System.Collections;

public class UserDefaultHandling : MonoBehaviour {
	
	public bool isIntroduced;

	private GazeDirectedGunHandling gunHandling;
	private GameObject mainCamera;
	private GameObject raycastObject;
	private GameObject welcomeMenu;
	private GameObject exitMenu;
	private GameObject scenesMenu;
	
	// Use this for initialization
	void Start ()
	{
		mainCamera = GameObject.Find ("Main Camera");
		welcomeMenu = GameObject.Find ("WelcomeMenu");
		exitMenu = GameObject.Find ("ExitConfirmationMenu");
		scenesMenu = GameObject.Find ("ScenesMenu");

		OVRTouchpad.Create ();
		OVRTouchpad.TouchHandler += HandleTouchHandler;
		
		welcomeMenu.SetActive (true);
		exitMenu.SetActive (false);
		scenesMenu.SetActive (false);
	
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
				scenesMenu.SetActive (false);

				MeshRenderer textMesh = GameObject.Find("YesExit").GetComponent<MeshRenderer>();
				textMesh.material.color = Color.white;
				textMesh = GameObject.Find("NoExit").GetComponent<MeshRenderer>();
				textMesh.material.color = Color.white;
			}
			
			if (raycastObject.name == "NoExit") {
				Debug.Log ("Single tap no exit text");
				welcomeMenu.SetActive (true);
				exitMenu.SetActive (false);
				scenesMenu.SetActive (false);
			}
			
			if (raycastObject.name == "YesExit") {
				Debug.Log ("Single tap yes exit text");
				Application.Quit ();
			}
			
			if (raycastObject.name == "EnterText") {
				Debug.Log ("Single tap enter text");
				welcomeMenu.SetActive (false);
				exitMenu.SetActive(false);
				scenesMenu.SetActive(true);
				isIntroduced = true;

				MeshRenderer textMesh = GameObject.Find("Snow").GetComponent<MeshRenderer>();
				textMesh.material.color = Color.white;
				textMesh = GameObject.Find("Maze").GetComponent<MeshRenderer>();
				textMesh.material.color = Color.white;
				textMesh = GameObject.Find("Desert").GetComponent<MeshRenderer>();
				textMesh.material.color = Color.white;
			}

			if (raycastObject.name == "Snow") {
				Debug.Log ("Single tap enter text");
				welcomeMenu.SetActive (false);
				exitMenu.SetActive(false);
				scenesMenu.SetActive(false);
				Application.LoadLevel("snow");
			}

			if (raycastObject.name == "Desert") {
				Debug.Log ("Single tap enter text");
				welcomeMenu.SetActive (false);
				exitMenu.SetActive(false);
				scenesMenu.SetActive(false);
				Application.LoadLevel("desert");
			}

			if (raycastObject.name == "Maze") {
				Debug.Log ("Single tap enter text");
				welcomeMenu.SetActive (false);
				exitMenu.SetActive(false);
				scenesMenu.SetActive(false);
				Application.LoadLevel("maze");
			}
			
		}
	}

	void Update () {

		Debug.Log ("is Introduced : " + isIntroduced);

		/* */
		if (Input.GetMouseButton (0)) {
			mainCamera.transform.Rotate (Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), 0);
		}

		Vector3 headPosition = mainCamera.transform.position;
		Vector3 gazeDirection = mainCamera.transform.forward;
		
		// Prepare to capture intersection data
		RaycastHit hitInfo;
		
		// Have the physics engine check for intersections with the vector originating at the head position
		// and heading in the direction of the user's gaze
		if (Physics.Raycast (headPosition, gazeDirection, out hitInfo)) {
			
			// Get the object being intersected
			raycastObject = hitInfo.transform.gameObject;
			
			Debug.Log (" Intersected element : " + hitInfo.transform.name);
			
			HandleAnimation();
		}
	}
	
	void HandleAnimation() {
		
		if (raycastObject.name == "ExitText") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;

			textMesh = GameObject.Find("EnterText").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}

		if (raycastObject.name == "EnterText") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;

			textMesh = GameObject.Find("ExitText").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}

		if (raycastObject.name == "YesExit") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;

			textMesh = GameObject.Find("NoExit").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}

		if (raycastObject.name == "NoExit") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;

			textMesh = GameObject.Find("YesExit").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}

		if (raycastObject.name == "Snow") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;
						
			textMesh = GameObject.Find("Maze").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;

			textMesh = GameObject.Find("Desert").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}

		if (raycastObject.name == "Maze") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;

			textMesh = GameObject.Find("Snow").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;

			textMesh = GameObject.Find("Desert").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}

		if (raycastObject.name == "Desert") {
			MeshRenderer textMesh = raycastObject.GetComponent<MeshRenderer>();
			textMesh.material.color = Color.green;
			
			textMesh = GameObject.Find("Snow").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;

			textMesh = GameObject.Find("Maze").GetComponent<MeshRenderer>();
			textMesh.material.color = Color.white;
		}
		
	}

}
