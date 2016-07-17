using UnityEngine;
using System.Collections;

public class pathTraverse : MonoBehaviour
{
	public float speed = 0.1F;
	public GameObject metaObject;
	private ArrayList glowingPathList = new ArrayList ();

	private Vector3 lastDetectedPathGameObjectPosition;

	void Start ()
	{
		glowingPathList.Add (metaObject);
		Update ();
	}

	// Update is called once per frame
	void Update ()
	{
		GameObject mainCameraGameObject = GameObject.Find ("Main Camera");
		Quaternion gazeDirection = mainCameraGameObject.transform.localRotation;

		// for input from button action only
		if (Input.GetMouseButton (1)) {
			ArrayList survivingPathList = new ArrayList ();

			for (int i = 0; i < glowingPathList.Count; i ++) {
				if (i > glowingPathList.Count - 2) {
					survivingPathList.Add (glowingPathList [i]);
					continue;
				}

				GameObject pathGameObject = (GameObject)glowingPathList [i];

				Vector3 deltaLocalPosition = lastDetectedPathGameObjectPosition - pathGameObject.transform.position;
			
				// Move the Navigation GameObject based on the travel vector
				transform.position = Vector3.Lerp (((GameObject)glowingPathList [i]).transform.position, 
				                                                   ((GameObject)glowingPathList [i + 1]).transform.position, 0.5f * Time.deltaTime)
					+ new Vector3 (0, 5, -9);
				lastDetectedPathGameObjectPosition = pathGameObject.transform.position;

				Destroy (pathGameObject);
			}
			glowingPathList.Clear ();
			glowingPathList = survivingPathList;
		}
		
		// for input from swipe action only
		else if (Input.GetMouseButton (0)) {
			GameObject glowingClone = null;

			Vector3 movedPosition = new Vector3 (Input.GetAxis ("Mouse X"), 0, Input.GetAxis ("Mouse Y"));
			//movedPosition = new Vector3 (movedPosition.x, 0, movedPosition.z);
			movedPosition = new Vector3(-movedPosition.z, 0, -movedPosition.x);

			GameObject lastDetectedGameObject = (GameObject) glowingPathList [glowingPathList.Count - 1];
			Vector3 lastDetectedGameObjectPosition = lastDetectedGameObject.transform.position;
		
			Vector3 newGlowingClonePosition = lastDetectedGameObjectPosition + movedPosition;

			double lastNotedDistance = (newGlowingClonePosition - lastDetectedGameObjectPosition).magnitude;

			if (lastNotedDistance > 0.5f && !isCollided (lastDetectedGameObjectPosition, movedPosition)) {
				glowingClone = Instantiate (lastDetectedGameObject, newGlowingClonePosition, lastDetectedGameObject.transform.rotation) as GameObject;
			
				glowingClone.name = "glowing-" + glowingPathList.Count;
				glowingPathList.Add (glowingClone);
			}
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

}
