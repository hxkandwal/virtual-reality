using UnityEngine;
using System.Collections;

// NOTE: We must add the Photon library
using ExitGames.Client.Photon;

// Script type: Singleton
// Functionality: Implements raycasting and handles multiplayer control
public class MultiplayerRaycasting : MonoBehaviour {
	
	// This region of functions handle connecting to the multiplayer server and game
	#region CONNECTION HANDLING
	
	// Initialize network settings before the application starts
	public void Awake () {
		// If not connected, set connection settings
		if (!PhotonNetwork.connected) {
			PhotonNetwork.autoJoinLobby = false;
			PhotonNetwork.ConnectUsingSettings("0.9");
		}
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnConnectedToMaster () {
		// Join a random game
		PhotonNetwork.JoinRandomRoom();
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnPhotonRandomJoinFailed () {
		// Create a random game if join attempt failed
		PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnJoinedRoom () {
		
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnCreatedRoom () {
		// Update the current scene based on the multiplayer information
		Application.LoadLevel(Application.loadedLevel);
	}
	
	#endregion
	
	// Inspector options for choosing multiplayer mode and raycasting options
	public enum MultiplayerMode {Fixed, Request, Takeover}
	[Header("Multiplayer Options")]
	public MultiplayerMode multiplayerMode;
	[Header("Raycasting Options")]



	public float swipeSpeed = 0.5f;			// How fast objects move forward/backward while swiping
	public float rayRadius = 0.025f;		// Radius of the ray object
	public float rayMaxLength = 100.0f;		// Maximum length of the ray object when not intersecting an object
	
	// Interaction states and variables
	bool initialized = false;				// Whether technique has been properly initialized
	bool objectRequested = false;				// Whether an object has been requested
	bool objectGrabbed = false;				// Whether an object is grabbed and being manipulated
	bool swipeReady = false;				// Whether touchpad input can be processed as a true swipe
	float graspDistance = 0.0f;				// How far the grasping point is from the main camera
	
	// Permanent references
	GameObject user = null;					// Reference to the User, which represents the user's position within the world
	GameObject hmd = null;					// Reference to the Main Camera, which represents the HMD
	GameObject grasp = null;				// Reference to the Grasp, which represents where the object was grabbed
	GameObject proxy = null;				// Reference to the Proxy, which represents the object during manipulation
	GameObject hand = null;					// Reference to the Hand, which represents where the user's hand would be
	GameObject ray = null;					// Reference to the Ray, which represents the ray
	Renderer rayRenderer = null;			// Reference to the Ray's renderer
	
	// Temporary references
	GameObject target = null;				// Reference to the GameObject being manipulated
	PhotonView targetView = null;			// Reference to the target's PhotonView (for multiplayer)
	
	// Handle any ownership requests for PhotonViews currently owned
	public void OnOwnershipRequest (object[] viewAndPlayer) {
		
		// Retrieve requested PhotonView and requesting PhotonPlayer
		PhotonView requestedView = viewAndPlayer[0] as PhotonView;
		PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
		
		// Ignore request if it comes from local client
		if(requestingPlayer.ID != PhotonNetwork.player.ID) {
			
			// Handle request based on multiplayer mode
			
			// Fixed mode (i.e., only the Master can interact)
			if(multiplayerMode == MultiplayerMode.Fixed) {
				
				// Ignore the request
			}
			// Request mode (i.e., non-target objects are handed over)
			else if(multiplayerMode == MultiplayerMode.Request) {
				
				// Ensure the requested object is not the current raycasting target
				if(requestedView != targetView) {
					requestedView.TransferOwnership(requestingPlayer.ID);
				}
			}
			// Takeover mode (i.e., any object is handed over, including current target)
			else if(multiplayerMode == MultiplayerMode.Takeover) {
				
				// Hand over the requested object, if it is not the current raycasting target
				if(requestedView != targetView) {
					requestedView.TransferOwnership(requestingPlayer.ID);
				}
				// Prepare and then hand over requested object, if it is the current raycasting target
				else {
					
					// Check if the target is grabbed
					if(objectGrabbed) {
						
						// Change our state back to not grabbing
						objectGrabbed = false;
						
						// Reset the object's kinematics
						if(target.GetComponent<Rigidbody>()) {
							target.GetComponent<Rigidbody>().isKinematic = true;
						}
						
						// Reset target and PhotonView
						target = null;
						targetView = null;
						
						// Color the ray green since we're still pointing at a manipulable object.
						// This avoids color blinking.
						rayRenderer.material.color = Color.green;
					}
					
					// Hand over the requested object
					requestedView.TransferOwnership(requestingPlayer.ID);
				}
			}
		}
	}
	
	
	// Use this for initialization
	void Start () {
		
		// Fetch object references
		user = GameObject.Find("User");
		hmd = GameObject.Find("HMD");
		grasp = GameObject.Find("Grasp");
		proxy = GameObject.Find("Proxy");
		hand = GameObject.Find("Hand");
		ray = GameObject.Find("Ray");
		
		// Fetch component references
		if(ray != null) {
			rayRenderer = ray.GetComponent<Renderer>();
		}
		
		// Initialize components
		if(rayRenderer != null) {
			rayRenderer.material.color = Color.blue;
		}
		
		// Verify that everything was properly initialized
		if(user != null && hmd != null && grasp != null && proxy != null && hand != null && ray != null && rayRenderer != null) {
			initialized = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		// Check that everything was properly initialized
		if(initialized) {
			
			// If an object is not grabbed
			if(!objectGrabbed) {
				
				// Update the ray based on the gaze direction
				UpdateRay();
				
				// Check if the user is requesting it with a touchpad press
				RequestGrab();
				
				// If an object has been requested
				if(objectRequested) {
					
					// Confirm requested object was handed over
					ConfirmGrab();
				}
			}
			// If an object is grabbed
			else {
				
				// Manipulate the object and ray based on the user's input
				ManipulateObject();
				
				// Then check if the user is releasing the object with a back button press
				CheckRelease();
			}
		}
	}
	
	// Update the ray based on the gaze direction
	void UpdateRay () {
		
		// Fetch the user's head position and gaze direction 
		Vector3 headPosition = user.transform.position;
		Vector3 gazeDirection = hmd.transform.forward;
		
		// Prepare to capture intersection data
		RaycastHit hitInfo;
		
		// Have the physics engine check for intersections with the vector originating at the head position
		// and heading in the direction of the user's gaze
		if(Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
			
			// Track the intersected object 
			target = hitInfo.transform.gameObject;
			
			// Move the grasp to the intersection point
			grasp.transform.position = hitInfo.point;
			
			// Match the proxy to the object's position
			proxy.transform.position = target.transform.position;
			proxy.transform.rotation = target.transform.rotation;
			
			// Determine the distance to the grasp
			graspDistance = (grasp.transform.position - headPosition).magnitude;
			
			// Now update the ray
			
			// As the hand position object is holding the ray, we'll point it to the intersected object
			hand.transform.LookAt(hitInfo.point);
			
			// Calculate the vector that will represent the ray and that vector's length
			Vector3 rayVector = hitInfo.point - hand.transform.position;
			float rayLength = rayVector.magnitude;
			
			// Scale and position the ray object accordingly to the hand position
			ray.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
			ray.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
			
			// Provide feedback whether the object can be manipulated
			// Only objects with the Manipulable tag can be manipulated
			if(target.tag == "Manipulable") {
				
				// Indicate manipulable objects with a green ray
				rayRenderer.material.color = Color.green;
			}
			// Not manipulable 
			else {
				
				// Indicate with a default blue ray
				rayRenderer.material.color = Color.blue;
			}
		}
		// No object was intersected by the user's gaze
		else {
			
			// Indicate a null object to ensure we don't try to fetch properties of it
			target = null;
			targetView = null;
			
			// Have the ray point in whatever direction the Main Camera is pointing
			hand.transform.localRotation = hmd.transform.rotation;
			
			// Have the ray extend out to the maximum length
			ray.transform.localPosition = new Vector3(0.0f, 0.0f, rayMaxLength/2.0f);
			ray.transform.localScale = new Vector3(rayRadius, rayMaxLength/2.0f, rayRadius);
			
			// Show the default blue ray
			rayRenderer.material.color = Color.blue;
			
			// Any request has been cancelled
			objectRequested = false;
		}
	}
	
	// Check to see if the user is requesting the object with a touchpad press
	void RequestGrab () {
		
		// If the touchpad is being pressed and an object is being intersected and it is manipulable
		if(Input.GetMouseButton(0) && target != null && target.tag == "Manipulable") {
			
			// Get target's PhotonView 
			targetView = target.GetComponent<PhotonView>();
			
			// Request ownership
			if(targetView != null) {
				objectRequested = true;
				targetView.RequestOwnership();
			}
		}
	}
	
	// Check to see if the user received ownership
	void ConfirmGrab () {
		
		// Check ownership
		if(targetView != null && targetView.isMine) {
			
			// Request has been completed
			objectRequested = false;
			
			// Change our state to grabbing
			objectGrabbed = true;
			
			// Turn off the object's kinematics to avoid the object from spinning in place due to collisions
			if(target.GetComponent<Rigidbody>()) {
				target.GetComponent<Rigidbody>().isKinematic = true;
			}
			
			// Color the ray yellow to indicate that an object is grabbed
			rayRenderer.material.color = Color.yellow;
		}
	}
	
	// Manipulate the object and ray based on the user's input
	void ManipulateObject () {
		
		// Ensure the user is swiping and not just touching the touchpad
		if(swipeReady) {
			
			// Get any forward or backward swipes from the touchpad
			float SwipeX = Input.GetAxis("Mouse X");
			
			// If there are any swipes
			if(Mathf.Abs(SwipeX) != 0.0f) {
				
				// Move the object forward or backward based on the swipe
				graspDistance -= SwipeX * swipeSpeed;
				
				// Keep the object a minimum distance of 0.5 meter away (i.e., arm's reach)
				if(graspDistance < 0.5f) {
					graspDistance = 0.5f;
				}
			}
		}
		
		// IMPORTANT: Swipes are based on the last touched position and the newest touched position.
		// Hence, if the user stopes touching the touchpad and then touches it again, a swipe from the
		// last touched point to the new touched point is generated. This will counteract any previous
		// input based on swipes, if you're not careful to check for this situation.
		
		// Check if the touchpad is being pressed
		if(Input.GetMouseButton(0)) {
			// Then we can accept a swipe next frame
			swipeReady = true;
		}
		// If the touchpad is not being pressed
		else {
			// We will not accept a swipe next frame
			swipeReady = false;
		}
		
		// Update the grasp position based on the new grasp distance
		Vector3 graspPosition = grasp.transform.localPosition;
		graspPosition.z = graspDistance;
		grasp.transform.localPosition = graspPosition;
		
		// Now update the target to match the proxy
		target.transform.position = proxy.transform.position;
		target.transform.rotation = proxy.transform.rotation;
		
		// Point the hand to the grasping location
		hand.transform.LookAt(grasp.transform.position);
		
		// Calculate the vector that will represent the ray and that vector's length
		Vector3 rayVector = grasp.transform.position - hand.transform.position;
		float rayLength = rayVector.magnitude;
		
		// Scale and position the ray object accordingly to the hand position
		ray.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
		ray.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
	}
	
	// Check if the user is releasing the object with a back button press
	void CheckRelease () {
		
		// Back button is pressed and there is an object being manipulated
		if(Input.GetMouseButton(1)) {
			
			// Change our state back to not grabbing
			objectGrabbed = false;
			
			// Reset the object's kinematics 
			if(target.GetComponent<Rigidbody>()) {
				target.GetComponent<Rigidbody>().isKinematic = false;
			}
			
			// Reset target and PhotonView
			target = null;
			targetView = null;
			
			// Color the ray green since we're still pointing at a manipulable object.
			// This avoids color blinking.
			rayRenderer.material.color = Color.green;
		}
	}
}
