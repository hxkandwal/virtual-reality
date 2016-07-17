using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VipFellow : MonoBehaviour {

	private GameObject user;
	private Slider userLifeline;
	public float E_life = 15.0f;
	public float max_life = 15.0f;
	public float  speed = 12.0f;
	public bool checkin = false;
	private bool isAttacking;
	private int attackImpactTime;
	public bool checkvip = true;
	
	// Use this for initialization
	void Start () {
		user = GameObject.Find ("User");
		userLifeline = GameObject.Find ("Lifeline").GetComponent<Slider>();
	}

	// Update is called once per frame
	void Update () {
		if (checkin) {
			if (checkvip) {
				RotateTo ();
				MoveTo ();
			}
		}
	}
	
	public void RotateTo ()  {
		float current = transform.eulerAngles.y;
		transform.LookAt (user.transform);
		
		Vector3 target = transform.eulerAngles;
		float next = Mathf.MoveTowardsAngle (current, target.y, 120 * Time.deltaTime);
		
		transform.eulerAngles = new Vector3 (0, next, 0);
	}
	
	public void MoveTo () {
		Vector3 pos1 = transform.position;
		Vector3 pos2 = user.transform.position;
		float dist = Vector2.Distance (new Vector2(pos1.x, pos1.z), new Vector2 (pos2.x, pos2.z));

		transform.GetComponent<Animation>().Play ("run");
		transform.Translate (new Vector3(0, 0, speed * Time.deltaTime));
	}
	
	void  OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "LightShootable") {
			checkin = true;
			transform.GetComponent<Animation>().Play ("run");
		}
	}
	
	void  OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "LighShootable") {
			userLifeline.value --;
		}
		if (other.gameObject.tag == "FPSplayer") {
			checkvip = false;
			transform.GetComponent<Animation> ().Play ("idle");
		}
	}
	
	void  OnTriggerExit (Collider other) {
		checkvip = true;
	}
	
	IEnumerator WaitAndGo() {
		yield return new WaitForSeconds(1);
	}
	
}
