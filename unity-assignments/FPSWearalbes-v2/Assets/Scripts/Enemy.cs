using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	private GameObject user;
	private Slider userLifeline;

	public float E_life = 15.0f;
	public float max_life = 15.0f;
	public float  speed = 12.0f;
	public bool checkin = true;
	private bool isAttacking;
	private int attackImpactTime;
	
	// Use this for initialization
	void Start () {
		user = GameObject.Find ("User");
		userLifeline = GameObject.Find ("Lifeline").GetComponent<Slider>();
	}

	// Update is called once per frame
	void Update () {
		if (checkin) {
			RotateTo ();
			MoveTo ();
		}
	}
	
	public void RotateTo ()  {
		float current = transform.eulerAngles.y;
		transform.LookAt (user.transform);

		Vector3 target = transform.eulerAngles;
		float next = Mathf.MoveTowardsAngle (current, target.y, 120 * Time.deltaTime);

		transform.eulerAngles = new Vector3 (0, next, 0);
		transform.GetComponent<Animation>().Play ("Run");
	}
	
	public void MoveTo () {
		Vector3 pos1 = transform.position;
		Vector3 pos2 = user.transform.position;
		//float dist = Vector2.Distance (new Vector2(pos1.x, pos1.z), new Vector2 (pos2.x, pos2.z));
		//transform.GetComponent<Animation>().Play ("Attack");
		transform.GetComponent<Animation>().Play ("Run");
		
		
		transform.Translate (new Vector3(0, 0, speed * Time.deltaTime));
	}

	void  OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "FPSplayer") {
			checkin = false;
			transform.GetComponent<Animation>().Play ("Attack");
		}
	}

	void  OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "FPSplayer") {
			checkin = false;

			if (! isAttacking) {
				transform.GetComponent<Animation>().Play ("Attack");
				isAttacking = true;
				attackImpactTime =  100;
				userLifeline.value --;
			} else {
				StartCoroutine(WaitAndGo());

				if (attackImpactTime > 0) {
					attackImpactTime --;
				} else {
					isAttacking = false;
				}
			}
		}
	}

	void  OnTriggerExit (Collider other) {
		checkin = true;
	}

	IEnumerator WaitAndGo() {
		yield return new WaitForSeconds(1);
	}

}
