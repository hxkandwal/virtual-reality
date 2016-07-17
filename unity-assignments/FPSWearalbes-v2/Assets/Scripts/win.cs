using UnityEngine;
using System.Collections;

public class win : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void  OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Vip") {
			Debug.Log(" win");
		}
	}
	
	void  OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Vip") {
			Debug.Log(" win");
		}
	}
}
