using UnityEngine;
using System.Collections;

public class fireeffect : MonoBehaviour {
	public GameObject Burst_bl;
	private Vector3 pos1;
	private GameObject ball;
	private GameObject ball1;
	// Use this for initialization
	void Start () {
		//pos1=this.transform.position;
		ball = GameObject.Find ("bb");
		ball1 = GameObject.Find ("bulletPosition");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Attack ();
		}
	}
	void Attack(){
		Debug.Log(" aaaa");

		GameObject burst=Instantiate(Burst_bl,ball.transform.position,Quaternion.identity)as GameObject;
		burst.transform.LookAt (ball1.transform.position);
		Destroy (burst,1.0f);
		
		
	}
}
