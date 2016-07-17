using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
	
	public float travelSpeed;
	public float walkingSpeed = 3;
	public float runningSpeed = 6;
	public float runningThreshold;
	public float forwardDampining=1f;
	public float sidewaysDampining=1f;
	public GameObject objectToMove;
	public GameObject mainCamera;
	public GameObject direction;
	private AndroidJavaObject unity = null;
	private AndroidJavaObject currentActivity = null;
	
	public bool crouching = false;
	private CharacterController control;
	public float crouchDeltaHeight = 0.1f;
	public float crouchingCamHeight = 1f;
	public float standardCamHeight = 2f;
	
	private bool shouldUpdate = false;
	private float previousZ = 0f;
	private float previousX = 0f;
	
	Quaternion previousDirection;
	
	private bool jump = false;
	public float jumpSpeed = 1f;
	public float jumpThreshold = 4f;
	
	public Vector3 gravity = Vector3.zero;
	public float gravityMultiplier = 1;
	
	// Use this for initialization
	void Start ()
	{
		objectToMove = GameObject.Find ("User");
		mainCamera = GameObject.Find ("Main Camera");
		direction = GameObject.Find ("Direction");
		control = objectToMove.GetComponent<CharacterController> ();
		
		runningThreshold = 4;
		travelSpeed = walkingSpeed;
		Debug.Log ("Getting the unity class");
		unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		Debug.Log ("Getting the main android class");
		currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		Debug.Log ("Getting data connection to android wear");
		currentActivity.Call ("dataConnectToWear");
		direction.transform.rotation = mainCamera.transform.rotation;
		
	}
	// Update is called once per frame
	void Update ()
	{
		
		int xAxis, zAxis;
		float yAxis = 0f;
		//bool crouch;
		if (currentActivity != null) {
			
			//Debug.Log("Updating the sensor values");
			currentActivity.Call ("updateSensors");
			Debug.Log ("Getting values from accelormeter");
			
			zAxis = currentActivity.Call<int> ("getCurrentAcceloX");
			xAxis = currentActivity.Call<int> ("getCurrentAcceloY");
			yAxis = 0f;
			//Debug.Log(zAxis+" "+xAxis+" "+yAxis);
			crouching = currentActivity.Call<bool> ("getTouchType");
			
			if (Mathf.Abs (xAxis) > runningThreshold || Mathf.Abs (zAxis) > runningThreshold)
				travelSpeed = runningSpeed;
			else
				travelSpeed = walkingSpeed;
			
		} else {
			int moveSpeed = 3;
			
			if(Input.GetKey(KeyCode.LeftShift))
				travelSpeed = runningSpeed;
			else
				travelSpeed =walkingSpeed;
			if (Input.GetKey ("w")) {
				//
				Debug.Log ("w");
				zAxis = moveSpeed;
			} else if (Input.GetKey ("s")) {
				//
				Debug.Log ("s");
				zAxis = -1*moveSpeed;
			} else {
				zAxis = 0;
			}
			if (Input.GetKey ("d")) {
				//
				Debug.Log ("d");
				xAxis = moveSpeed;
			} else if (Input.GetKey ("a")) {
				//
				Debug.Log ("a");
				xAxis = -1*moveSpeed
					;
			} else {
				xAxis = 0;
			}
			if (Input.GetKey ("c")) {
				//
				Debug.Log ("c");
				crouching = true;
			} else {
				crouching =false;
			}
			
			if(Input.GetKeyDown("e")){
				//control.transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime, Space.World);
				jump = true;
			}
			
		}
		
		updateCrouch ();
		
		Vector3 motionVector;
		if (jump) {
			motionVector = new Vector3 (0, yAxis,0);
		} else {
			motionVector = new Vector3 ((xAxis) / sidewaysDampining, yAxis, (zAxis * Mathf.Abs (zAxis)) / forwardDampining);
		}
		// Debug.Log (previousX + " " + previousZ);
		if (previousX == 0 && previousZ == 0) {
			shouldUpdate = true;
		} else {
			shouldUpdate = false;
		}
		
		
		if(shouldUpdate){
			direction.transform.rotation = mainCamera.transform.rotation;
		}else{
			direction.transform.rotation = previousDirection;
		}
		
		
		motionVector.Normalize ();
		motionVector = direction.transform.TransformDirection (motionVector);
		if (crouching) {
			travelSpeed = walkingSpeed/2;
		}
		
		//Jumping
		if (Mathf.Abs (previousZ - zAxis) > jumpThreshold) {
			//control.transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime, Space.World);
			jump = true;
		}
		
		motionVector *= travelSpeed;
		if (!control.isGrounded) {
			gravity += (Physics.gravity * gravityMultiplier) * Time.deltaTime;
			
		} else {
			gravity = Vector3.zero;
			
			if(jump){
				Debug.Log("Jumping");
				Debug.Log("DHHHHHHHHHHHHHH***************");
				gravity.y = jumpSpeed;
				jump = false;
			}
		}
		
		motionVector += gravity;
		
		//control.SimpleMove (motionVector * travelSpeed);
		control.Move(motionVector * Time.deltaTime);
		
		previousZ = zAxis;
		previousX = xAxis;
		previousDirection = direction.transform.rotation;
		
	}
	
	
	
	
	void updateCrouch ()
	{
		if (crouching) {
			if (control.height > crouchingCamHeight) {
				
				control.height -= crouchDeltaHeight;
			}
		} else if (control.height < standardCamHeight) {
			control.height += crouchDeltaHeight;
		}
	}
	
}