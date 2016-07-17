using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour {
	public int speed=5;
	//public Transform newobject;
	//public int sdshu =0;
	//public static int xmshu  =0;
	// Use this for initialization
	void Start () {
		int speed=5;
		 Transform newobject;
		 
	}
	
	// Update is called once per frame
	void Update () {
		float x=-Input.GetAxis("Horizontal")*Time.deltaTime*speed;
		float z=-Input.GetAxis("Vertical")*Time.deltaTime*speed;
		transform.Translate(x,0,z);
		//if(Input.GetButtonDown("Fire1")){
			//Transform n= (Transform)Instantiate(newobject,transform.position,transform.rotation);
			//Vector3 fwd=transform.TransformDirection(Vector3.forward);
			//n.GetComponent<Rigidbody>().AddForce(fwd*5000);
			//sdshu++;
			//Destroy(n.gameObject,5);

			//GameObject.Find("Text").GetComponent<TextMesh>().text="sdshu:"+sdshu+"xmshu"+xmshu;
		}
	void OnTigger(){}
	}

