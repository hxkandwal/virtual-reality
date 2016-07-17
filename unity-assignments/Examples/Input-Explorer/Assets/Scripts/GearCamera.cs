using UnityEngine;
using System.Collections;

// The Camera is located on the backside of the Gear VR. It's resolution is 3840 x 2160.

public class GearCamera : MonoBehaviour {

	// Define a camera texture
	WebCamTexture BackCamera;

	// Use this for initialization
	void Start () {
		
		// Android defaults to back camera	
		BackCamera = new WebCamTexture();
		// Set the texture of the gameobject to the camera texture		
		gameObject.GetComponent<Renderer>().material.mainTexture = BackCamera;
		// Keep the texture updating
		BackCamera.Play();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// Flip the texture upright
		gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1.0f, 1.0f);
		// Move the texture down
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.0f);
		
	}
	
}
