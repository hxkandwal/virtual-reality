using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy3 : MonoBehaviour {

	private GameObject user;
	private Slider userLifeline;
	public PathNode m_currentNode;
	public float E_life = 15.0f;
	public float max_life = 15.0f;
	public float  speed = 5.0f;
	public bool checkin = true;
	public bool isBeingAttacked;
	private bool isAttacking;
	private int attackImpactTime;
	private Rigidbody enemyRigidBody;
	private int hitcount = 1;

	// Use this for initialization
	void Start () {
		enemyRigidBody = gameObject.GetComponent<Rigidbody>();
		user = GameObject.Find ("User");
		userLifeline = GameObject.Find ("Lifeline").GetComponent<Slider>();
	}

	public void DisableAttack() {
		hitcount ++;
		StartCoroutine (WaitAndGo (hitcount * 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		if (checkin && !isBeingAttacked) {
			RotateTo ();
			MoveTo ();
		}
	}

	public void RotateTo(){
		float current = transform.eulerAngles.y;
		this.transform.LookAt (m_currentNode.transform);
		Vector3 target=this.transform.eulerAngles;
		float next = Mathf.MoveTowardsAngle (current,target.y,120*Time.deltaTime);
		this.transform.eulerAngles = new Vector3 (0,next,0);
		transform.GetComponent<Animation>().Play ("run");
	}

	public void MoveTo() {
		Vector3 pos1 = transform.position;
		Vector3 pos2 = m_currentNode.transform.position;
		float dist = Vector2.Distance (new Vector2 (pos1.x,pos1.z),new Vector2 (pos2.x,pos2.z));
			
		transform.Translate (new Vector3 (0,0, speed * Time.deltaTime));
		transform.GetComponent<Animation>().Play ("run");
	}

	void  OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Vip") {
			checkin = false;
			transform.GetComponent<Animation>().Play ("attack01");
		}
	}
	
	void  OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Vip") {
			checkin = false;
			
			if (! isAttacking) {
				transform.GetComponent<Animation>().Play ("attack01");
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

	IEnumerator WaitAndGo(float time) {   
		isBeingAttacked = true;
		transform.GetComponent<Animation> ().Play ("damage02");
		yield return new WaitForSeconds(time);
		isBeingAttacked = false;
	}

}
