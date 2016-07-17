using UnityEngine;
using System.Collections;

public class doorOpen : MonoBehaviour {

	public GameManager gameManager;
	private GameObject user;
	private GameObject lerpz;

	void Start () {
		user = GameObject.Find ("User");
		lerpz = GameObject.Find ("Lerpz");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void  OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "FPSplayer") {
			StartCoroutine (WaitAndGo());
		}

		if (other.gameObject.tag == "Vip") {
			gameManager.GameEnd (true);
		}
	}

	void  OnTriggerStay (Collider other) {}

	IEnumerator WaitAndGo() {   
		yield return new WaitForSeconds(3);
		transform.GetComponent<Animation>().Play ("Open");
		user.transform.LookAt(lerpz.transform);
	}
}
