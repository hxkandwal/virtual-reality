using UnityEngine;
using System.Collections;

// Note:
// Attach to any GameObject that should be synchronized during multiplayer

// Ensure a RigidBody and PhotonView is attached to the GameObject
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]

public class SyncRigidBody : MonoBehaviour {

	// Inspector options to choose what to synchronize
	public bool syncMass = false;
	public bool syncDrag = false;
	public bool syncAngularDrag = false;
	public bool syncUseGravity = true;
	public bool syncIsKinematic = true;
	
	// While this script is observed in a PhotonView, this function is called by PUN	
	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		
		// Get a reference to the required Rigidbody
		Rigidbody rigidBody = GetComponent<Rigidbody>();
		
		// If the local client is the owner of the PhotonView, the stream will be set to write
		if (stream.isWriting) {
		
			// If suppose to synchronize Mass
			if(syncMass) {
				// Fetch the Mass
				float mass = rigidBody.mass;
				// Write the Mass to stream
				stream.Serialize(ref mass);
			}
			
			// If suppose to synchronize Drag
			if(syncDrag) {
				// Fetch the Drag
				float drag = rigidBody.drag;
				// Write the Drag to stream
				stream.Serialize(ref drag);
			}
			
			// If suppose to synchronize Angular Drag
			if(syncAngularDrag) {
				// Fetch the Angular Drag
				float angularDrag = rigidBody.angularDrag;
				// Write the Angular Drag to stream
				stream.Serialize(ref angularDrag);
			}
			
			// If suppose to synchronize Use Gravity
			if(syncUseGravity) {
				// Fetch the Use Gravity
				bool useGravity = rigidBody.useGravity;
				// Write the Use Gravity to stream
				stream.Serialize(ref useGravity);
			}
			
			// If suppose to synchronize Is Kinematic
			if(syncIsKinematic) {
				// Fetch the Is Kinematic
				bool isKinematic = rigidBody.isKinematic;
				// Write the Is Kinematic to stream
				stream.Serialize(ref isKinematic);
			}
		}
		// If the local client is not the owner of the PhotonView, the stream will be set to read
		else {
			
			// If suppose to synchronize Mass
			if(syncMass) {
				// Prepare to receive the Mass
				float mass = 0.0f;
				// Read the Mass from stream
				stream.Serialize(ref mass);
				// Update the Mass
				rigidBody.mass = mass;
			}
			
			// If suppose to synchronize Drag
			if(syncDrag) {
				// Prepare to receive the Drag
				float drag = 0.0f;
				// Read the Drag from stream
				stream.Serialize(ref drag);
				// Update the Drag
				rigidBody.drag = drag;
			}
			
			// If suppose to synchronize Angular Drag
			if(syncAngularDrag) {
				// Prepare to receive the Angular Drag
				float angularDrag = 0.0f;
				// Read the Angular Drag from stream
				stream.Serialize(ref angularDrag);
				// Update the Angular Drag
				rigidBody.angularDrag = angularDrag;
			}
			
			// If suppose to synchronize Use Gravity
			if(syncUseGravity) {
				// Prepare to receive the Use Gravity
				bool useGravity = false;
				// Read the Use Gravity from stream
				stream.Serialize(ref useGravity);
				// Update the Use Gravity
				rigidBody.useGravity = useGravity;
			}
			
			// If suppose to synchronize Is Kinematic
			if(syncIsKinematic) {
				// Prepare to receive the Is Kinematic
				bool isKinematic = false;
				// Read the Is Kinematic from stream
				stream.Serialize(ref isKinematic);
				// Update the Is Kinematic
				rigidBody.isKinematic = isKinematic;
			}
		}
	}
}
