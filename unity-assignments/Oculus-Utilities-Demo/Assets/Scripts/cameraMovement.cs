using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cameraMovement : MonoBehaviour {

	public GameObject displayTextGameObject;
	private Text text;

	// Use this for initialization
	void Start () {
		text = displayTextGameObject.GetComponent<Text> ();
		text.text = "";

		OVRTouchpad.Create();
		OVRTouchpad.TouchHandler += HandleTouchHandler;
	}

	void HandleTouchHandler (object sender, System.EventArgs e)
	{
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs) e;

		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap) {
			text.text = "Single tap !";
		}
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Up) {
			text.text = "Going Up !";
		}
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Down) {
			text.text = "Heading Down !";
		}
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Right) {
			text.text = "Going right !";
		}
		if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Left) {
			text.text = "Going Left !";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Vector3 movedPosition = new Vector3 (Input.GetAxis ("Mouse X"), 0, Input.GetAxis ("Mouse Y"));
			movedPosition = new Vector3(-movedPosition.z, 0, -movedPosition.x);

			transform.position += movedPosition;
		}
	}
}