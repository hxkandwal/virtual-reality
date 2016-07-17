using UnityEngine;
using System.Collections;

public class GazeDirectedGunHandling : MonoBehaviour {
	
	private GameObject gameCamera;
	private GameObject user;
	private GameObject hand;
	private GameObject lockedCrosshair;
	private GameObject hitObject;
	private GameObject fire_shoot;
	public GameObject Burst_bl;
	public Enemy3 zombie;
	public ControlBox snake;

	private Vector3 pos1;
	private GameObject ball;
	private GameObject ball1;

	void Start () {
		user = GameObject.Find ("User");
		gameCamera = GameObject.Find ("Main Camera");
		lockedCrosshair = GameObject.Find ("Target Lock");
		hand = GameObject.Find ("Hand");
		fire_shoot = GameObject.Find ("Fireshoot");
		ball = GameObject.Find ("bb");
		ball1 = GameObject.Find ("bulletPosition");
	
		fire_shoot.GetComponent<Renderer>().enabled = false;
		lockedCrosshair.SetActive (false);

		OVRTouchpad.Create();
		OVRTouchpad.TouchHandler += HandleTouchHandler;
	}

	void HandleTouchHandler (object sender, System.EventArgs e) {

		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs) e;

		// If the touchpad is being pressed and an object is being intersected and it is manipulable
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap) {

			StartCoroutine (Attackfire());
			bulletfire ();

			if (hitObject != null && hitObject.tag == "Shootable") {
				hitObject.SetActive (false);
			}

			if (hitObject != null && hitObject.tag == "DecrementShootable") {
				Debug.Log("Disabling Eating");
				snake.DisableEating();
			}

			if (hitObject != null && hitObject.tag == "LightShootable") {
				Debug.Log("Disabling Attack");
				zombie.DisableAttack();
			}

			if (hitObject != null && hitObject.tag == "GameEnd") {
				Debug.Log ("Loading new level");
				Application.LoadLevel("ControlScene");
			}

			StartCoroutine (Gunshake());
		}

	}

	// Update is called once per frame
	void Update () {

		hitObject = null;

		/** 
		 * Hack for camera handling, need to be removed !
		 * 
*/
		if (Input.GetMouseButton (0)) {
			gameCamera.transform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
		}
		


		if (Application.loadedLevelName == "snow") {
			Quaternion cameraDirection = gameCamera.transform.localRotation;
			hand.transform.localRotation = cameraDirection;
		}
		
		RaycastHit interceptedObject;

		if (Physics.Raycast(user.transform.position, gameCamera.transform.forward, out interceptedObject)) {

			Debug.Log(" In Range :  " +  interceptedObject.transform.name);

			if (interceptedObject.transform.tag.Contains("Shootable"))
			{
				Debug.Log(" Yes this object is shootable ");
				lockedCrosshair.SetActive (true);
				lockedCrosshair.transform.Rotate (0, 0, 30 * Time.deltaTime * 2f);

				hitObject = interceptedObject.transform.gameObject;
			}
			else {
				if (interceptedObject.transform.tag == "GameEnd")
					hitObject = interceptedObject.transform.gameObject;

				Debug.Log ("Setting false");
				lockedCrosshair.SetActive (false);
			}
		}
	}

	public IEnumerator Attackfire () {
		fire_shoot.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds (0.2f);
		fire_shoot.GetComponent<Renderer>().enabled = false;
	}

	public IEnumerator Gunshake () {
		if (Application.loadedLevelName == "desert") {
			hand.transform.Translate (0f, 0f, -0.5f);
			yield return new WaitForSeconds (0.3f);
			hand.transform.Translate (0f, 0f, 0.5f);
		} else {
			hand.transform.Translate (0f, 0.1f, -0.005f);
			yield return new WaitForSeconds (0.2f);
			hand.transform.Translate (0f, -0.1f, 0.005f);
		}
	}

	public void bulletfire() {
		if (Burst_bl != null) {
			GameObject burst = Instantiate (Burst_bl, ball.transform.position, Quaternion.identity) as GameObject;
			burst.transform.LookAt (ball1.transform.position);
			Destroy (burst, 1.0f);
		}
	}
}
