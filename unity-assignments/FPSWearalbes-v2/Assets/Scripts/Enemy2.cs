using UnityEngine;
using System.Collections;

public class Enemy2 : MonoBehaviour {

	public GameObject user;
	//private Slider userLifeline;
	public float E_life = 15.0f;
	public float max_life = 15.0f;
	public float  speed = 12.0f;
	public bool checkin = true;
	private bool isAttacking;
	private int attackImpactTime;
	public GameObject Burst_bl;
	private Vector3 pos1;
	private GameObject ball;
	private GameObject ball1;
	
	// Use this for initialization
	void Start () {
		user = GameObject.Find ("Lerpz");
		ball = GameObject.Find ("shoottarget");
		ball1 = GameObject.Find ("firePoint");
		//userLifeline = GameObject.Find ("Lifeline").GetComponent<Slider>();
		//userLifeline.onValueChanged.AddListener (ListenerMethod);
	}
	

	
	// Update is called once per frame
	void Update () {
		if (checkin) {
			RotateTo ();
			MoveTo ();
		}
		//OnTriggerEnter ();
	}
	
	public void RotateTo ()  {
		float current = transform.eulerAngles.y;
		transform.LookAt (user.transform);
		
		Vector3 target = transform.eulerAngles;
		float next = Mathf.MoveTowardsAngle (current, target.y, 120 * Time.deltaTime);
		
		transform.eulerAngles = new Vector3 (0, next, 0);
		//transform.GetComponent<Animation>().Play ("Attack");
		//transform.GetComponent<Animation>().Play ("PA_DroneLeanFoward_Clip");
	}
	
	public void MoveTo () {
		Vector3 pos1 = transform.position;
		Vector3 pos2 = user.transform.position;
		float dist = Vector2.Distance (new Vector2(pos1.x, pos1.z), new Vector2 (pos2.x, pos2.z));
		//transform.GetComponent<Animation>().Play ("Attack");
		//transform.GetComponent<Animation>().Play ("PA_DroneLeanFoward_Clip");
		
		
		transform.Translate (new Vector3(0, 0, speed * Time.deltaTime));
	}
	
	void  OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "shootT") {
			checkin = false;
			Attack();;
		}
	}
	
	void  OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "shootT") {
			checkin = false;
			
			if (! isAttacking) {
				Attack();
				isAttacking = true;
				attackImpactTime =  100;
				//userLifeline.value --;
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

	void Attack(){
		GameObject burst=Instantiate(Burst_bl,ball.transform.position,Quaternion.identity)as GameObject;
		burst.transform.LookAt (ball1.transform.position);
		Destroy (burst,1.0f);
	}

	IEnumerator WaitAndGo()
	{
		yield return new WaitForSeconds(1);
	}
}
