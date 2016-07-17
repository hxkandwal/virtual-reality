using UnityEngine;
using System.Collections;

public class GazeDirectedGunHandling : MonoBehaviour {

	public GameObject birdCollider;
	private GameObject gameCamera;
	private GameObject user;
	private GameObject hand;
	private GameObject hitObject;

	void Start () {
		user = GameObject.Find ("User");
		gameCamera = GameObject.Find ("Main Camera");
		hand = GameObject.Find ("Hand");

		OVRTouchpad.Create();
		OVRTouchpad.TouchHandler += HandleTouchHandler;
	}

	void HandleTouchHandler (object sender, System.EventArgs e)
	{
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs) e;

		// If the touchpad is being pressed and an object is being intersected and it is manipulable
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap && hitObject != null && hitObject.tag == "Shootable") {
			hitObject.SetActive(false);
		}

	}

	// Update is called once per frame
	void Update () {

		hitObject = null;

		/** 
		 * Hack for camera handling, need to be removed !
		 * */
		if (Input.GetMouseButton (0)) {
			gameCamera.transform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
		}

		Quaternion cameraDirection = gameCamera.transform.localRotation;
		hand.transform.localRotation = cameraDirection;

		RaycastHit interceptedObject;

		if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out interceptedObject)) {

			Debug.Log (" name is : " + interceptedObject.transform.name);

			if (interceptedObject.transform.tag == "Shootable")
			{
				hitObject = interceptedObject.transform.gameObject;
				birdCollider.transform.position = interceptedObject.transform.position;
			}
			else {
				birdCollider.transform.position = user.transform.position;
			}
		} 
		else {
			birdCollider.transform.position = user.transform.position;
		}

	}
}
