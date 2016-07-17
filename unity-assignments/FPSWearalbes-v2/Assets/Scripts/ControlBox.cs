using UnityEngine;
using System.Collections;
using System;

public class ControlBox : MonoBehaviour {
	GameObject snake = null;
	int directionChangeCounter = -1;
	private bool beingHandled = false;
	int lastFrameUpdateTimeInSec = 61;
	float lastClickHandled = 61;
	ArrayList moverList = new ArrayList();

	private bool eatingAllowed = true;
	private int hitCount;
			 //	x		z
			 //85		-80		x1
			//-85		-80		x2
			//-85		 50		x3
			// 85		 50		x4
			// x3 -> x2  -> x1 -> x4 ->x3

//	int moverIndex = 0;
//	int numberOfBoxes = 3;

	// Use this for initialization
	void Start () {
		Debug.Log ("Hi, Welcome to SnakeGame");

		Debug.Log ("******************************************************************************jhbjhvjhv");
		GameObject objectToMove = GameObject.Find ("User");
		objectToMove.GetComponent<Rigidbody> ().useGravity = false;

		snake = GameObject.Find ("Snake");
		Mover mover = new Mover();
		mover.emptyGameObject = transform.GetChild (0).gameObject;
		//Vector3 temp = new Vector3(0, 2, 0);
		//mover.initialPosition = temp;
		mover.directionOfMotion = new Vector3 (0, 0, -3);
		moverList.Add(mover);
	/*	GameObject randomCube = GameObject.Find ("randomCube");
		print ("randomCube.transform.position:" + randomCube.transform.position.ToString ());
	*/}
	
	// Update is called once per frame
	void Update () {
		if (eatingAllowed) {
			Boolean updateFrame = false;
			int msecond = DateTime.Now.Millisecond;
			msecond = msecond / 50;
			if (msecond != lastFrameUpdateTimeInSec)
				updateFrame = true;

			if (updateFrame) {

				Mover frontMover = (Mover)moverList [moverList.Count - 1];
				GameObject frontMoverFirstCube = frontMover.emptyGameObject.transform.GetChild (0).gameObject;
				//Debug.Log ("Value of x:" + frontMoverFirstCube.transform.position.x);
				//Debug.Log ("Value of z:" + frontMoverFirstCube.transform.position.z);
				if (frontMoverFirstCube.transform.position.x == 85 && frontMoverFirstCube.transform.position.z == -80) {
					changeH (-1);
				}
				if (frontMoverFirstCube.transform.position.x == -85 && frontMoverFirstCube.transform.position.z == -80) {
					changeH (-1);
				}
				if (frontMoverFirstCube.transform.position.x == -85 && frontMoverFirstCube.transform.position.z == 50) {
					changeH (-1);
				}
				if (frontMoverFirstCube.transform.position.x == 85 && frontMoverFirstCube.transform.position.z == 50) {
					changeH (-1);
				}

				directionChangeCounter++;
				if (moverList.Count > 1) {
					foreach (Mover m in moverList) {
						m.emptyGameObject.transform.Translate ((m.directionOfMotion / 3));
					}

					int listSize = moverList.Count;

					if (directionChangeCounter >= 0 && directionChangeCounter % 3 == 0)
						for (int i=0; i<listSize-1; i++) {
							directionChangeCounter = 0;
							Mover m = (Mover)moverList [i];
							GameObject fstChild = m.emptyGameObject.transform.GetChild (0).gameObject;
							fstChild.transform.SetParent (((Mover)moverList [i + 1]).emptyGameObject.transform, true);

							if (m.emptyGameObject.transform.childCount == 0) {
								GameObject.Destroy (((Mover)moverList [i]).emptyGameObject);
								moverList.RemoveAt (i);
								listSize = moverList.Count;
								i--;
							}
						}
				} else {
					foreach (Mover m in moverList) {
						if (m.emptyGameObject.transform.childCount > 0)
							m.emptyGameObject.transform.Translate (m.directionOfMotion / 3);
					}
				}
				updateFrame = false;
				lastFrameUpdateTimeInSec = msecond;

				//cube moved and hence check if first cube is in same position as random cube if yes append it to the end
				handleIntersection ();
			}

			float horizontalClick = Input.GetAxis ("Horizontal");
			/*if (horizontalClick != 0) {
			msecond = DateTime.Now.Millisecond;
			if(msecond != lastClickHandled){
				changeH(horizontalClick);
				lastClickHandled = msecond;
			}
		}

		float verticalClick = Input.GetAxis("Vertical");
		if (verticalClick != 0) {
			msecond = DateTime.Now.Millisecond;
			if(msecond != lastClickHandled){
				changeV(verticalClick);
				lastClickHandled = msecond;
			}
		}*/

		}
	}
	
	void introduceCube()
	{
		//introduce cube at random location
		//make it chaid of firstMover
		//5 5 -5 
		System.Random r = new System.Random ();
		int x, y, z, random;
		y = 10;
		random = r.Next (1,3);
		if (random == 1) {
			random = r.Next (1,3);
			if(random == 1)
			{
				z = r.Next (-80, 50);
				x = 85;
			}
			else{
				z = r.Next (-80, 50);
				x = -85;
			}
		} else {
			random = r.Next (1,3);
			if(random == 1)
			{
				x = r.Next (-85, 80);
				z = 50;
			}
			else{
				x = r.Next (-85, 80);
				z = -80;
			}
		}

		//Debug.Log ("rand x:" + x);
		//Debug.Log ("rand z:" + z);

		Vector3 pos = new Vector3 (x, y, z);
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject parent = GameObject.Find ("Snake");
		cube.transform.localScale += new Vector3(2, 2, 2);
		cube.transform.Translate (pos);
		cube.tag = "Shootable";
		cube.name="randomCube";
	}

	public void DisableEating() {
		hitCount ++;
		StartCoroutine (WaitAndGo (0.5f));

		if (hitCount % 5 == 0) {
			Debug.Log(" Hit count : " + hitCount);
			Mover frontMover = (Mover)moverList [moverList.Count - 1];
			GameObject frontMoverFirstCube = frontMover.emptyGameObject.transform.GetChild (0).gameObject;
			Destroy (frontMoverFirstCube);
		}
	}

	//cube moved and hence check if first cube is in same position as random cube if yes append it to the end
	void handleIntersection()
	{
		Mover frontMover = (Mover)moverList [moverList.Count - 1];
		GameObject frontMoverFirstCube = frontMover.emptyGameObject.transform.GetChild (0).gameObject;
		GameObject randomCube = GameObject.Find ("randomCube");
		//print ("frontMoverFirstCube.transform.position:" + frontMoverFirstCube.transform.position.ToString ());

		if (randomCube != null) {
			Vector3 fromFirstCubeToRandomCube = frontMoverFirstCube.transform.position - randomCube.transform.position;

			if (fromFirstCubeToRandomCube.magnitude < 1.5) {
				print ("interesection happened");
				int x = 0, y = 0, z = 0;
				Mover lastMover = (Mover)moverList [0];
				GameObject lastCube = lastMover.emptyGameObject.transform.GetChild (lastMover.emptyGameObject.transform.childCount - 1).gameObject;
				print (lastCube.name);
				randomCube.transform.SetParent (lastMover.emptyGameObject.transform, true);
				/*	if(lastCube.transform.position.x !=0)
			{
				x = -1*lastMover.emptyGameObject.transform.childCount;
			}
			else if(lastCube.transform.position.x !=0)
			{
				y = -1*lastMover.emptyGameObject.transform.childCount;
			}
			else if(lastCube.transform.position.y !=0)
			{
				z = -1*lastMover.emptyGameObject.transform.childCount;
			}*/
				Vector3 position = new Vector3 (x, y, z);
				position = lastCube.transform.position - lastMover.directionOfMotion;
				randomCube.transform.position = position;
				randomCube.name = "target";
				randomCube.tag = "DecrementShootable"; 
				//introduce new cube at random position
			}
		} else {
			introduceCube ();
		}
	}

	void changeV(float v)
	{
		Mover frontMover = (Mover)moverList[moverList.Count-1];
		GameObject lastMoverFirstCube = frontMover.emptyGameObject.transform.GetChild (0).gameObject;
		Vector3 lastMoverDirection = frontMover.directionOfMotion;

		Mover newMover = new Mover();
		newMover.emptyGameObject = new GameObject();
		newMover.emptyGameObject.name = "mover2";
		newMover.emptyGameObject.transform.position = frontMover.emptyGameObject.transform.position;
		newMover.emptyGameObject.transform.SetParent (snake.transform, true);
		lastMoverFirstCube.transform.SetParent(newMover.emptyGameObject.transform, true);
		moverList.Add (newMover);
		if (v > 0) //hopefully up arrow ;-)	
		{
			if(frontMover.directionOfMotion.z == -3 || frontMover.directionOfMotion.z == 3)
			{
				newMover.directionOfMotion = new Vector3(0, 3, 0);
			}
			else if(frontMover.directionOfMotion.x ==-3 || frontMover.directionOfMotion.x ==3)
			{
				newMover.directionOfMotion = new Vector3(0 ,3 ,0);
			}
			else if(frontMover.directionOfMotion.y == 3 || frontMover.directionOfMotion.y == -3)
			{
				newMover.directionOfMotion = new Vector3(0 ,0 ,-3);
			}
			else
				Debug.Log("***************Something wrong should not have reached here**************");
		} else 
		{
			if(frontMover.directionOfMotion.z == -3 || frontMover.directionOfMotion.z == 3)
			{
				newMover.directionOfMotion = new Vector3( 0, -3, 0);
			}
			else if(frontMover.directionOfMotion.x ==-3 || frontMover.directionOfMotion.x ==3)
			{
				newMover.directionOfMotion = new Vector3(0 ,-3 ,0);
			}
			else if(frontMover.directionOfMotion.y == 3 || frontMover.directionOfMotion.y == -3)
			{
				newMover.directionOfMotion = new Vector3(0 ,0 ,3);
			}
			else
				Debug.Log("***************Something wrong should not have reached here**************");
		}
	}

	void changeH(float h)
	{
		h = - 1;
		directionChangeCounter = 0;
		Mover frontMover = (Mover)moverList [moverList.Count - 1];
		GameObject lastMoverFirstCube = frontMover.emptyGameObject.transform.GetChild (0).gameObject;
		Vector3 lastMoverDirection = frontMover.directionOfMotion;

		//creating new mover
		Mover newMover = new Mover ();
		newMover.emptyGameObject = new GameObject ();
		newMover.emptyGameObject.name = "mover2";
		newMover.emptyGameObject.transform.position = frontMover.emptyGameObject.transform.position;
		newMover.emptyGameObject.transform.SetParent (snake.transform, true);
		lastMoverFirstCube.transform.SetParent (newMover.emptyGameObject.transform, true);
		moverList.Add (newMover);
		if (h > 0) {//right
			if (frontMover.directionOfMotion.z == -3) {
				newMover.directionOfMotion = new Vector3 (-3, 0, 0);
			} else if (frontMover.directionOfMotion.z == 3) {
				newMover.directionOfMotion = new Vector3 (3, 0, 0);
			} else if (frontMover.directionOfMotion.x == -3) {
				newMover.directionOfMotion = new Vector3 (0, 0, 3);
			} else if (frontMover.directionOfMotion.x == 3) {
				newMover.directionOfMotion = new Vector3 (0, 0, -3);
			} else if (frontMover.directionOfMotion.y == -3) {
				newMover.directionOfMotion = new Vector3 (-3, 0, 0);
			} else if (frontMover.directionOfMotion.y == 3) {
				newMover.directionOfMotion = new Vector3 (-3, 0, 0);
			} else
				Debug.Log ("***************Something wrong should not have reached here**************");
		} else {//left
			if (frontMover.directionOfMotion.z == -3) {
				newMover.directionOfMotion = new Vector3 (3, 0, 0);
			} else if (frontMover.directionOfMotion.z == 3) {
				newMover.directionOfMotion = new Vector3 (-3, 0, 0);
			} else if (frontMover.directionOfMotion.x == -3) {
				newMover.directionOfMotion = new Vector3 (0, 0, -3);
			} else if (frontMover.directionOfMotion.x == 3) {
				newMover.directionOfMotion = new Vector3 (0, 0, 3);
			} else if (frontMover.directionOfMotion.y == -3) {
				newMover.directionOfMotion = new Vector3 (3, 0, 0);
			} else if (frontMover.directionOfMotion.y == 3) {
				newMover.directionOfMotion = new Vector3 (3, 0, 0);
			} else
				Debug.Log ("***************Something wrong should not have reached here**************");
		}
	}

	IEnumerator WaitAndGo(float time)
	{   eatingAllowed = false;
		yield return new WaitForSeconds(time);
		eatingAllowed = true;
	}

	class Mover{
		public GameObject emptyGameObject;
		public Vector3 directionOfMotion;
	}
}
