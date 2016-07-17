using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BirdDisplayHandling : MonoBehaviour {

	private ArrayList childBirds;
	public GameObject scoreHitCount;	

	// Use this for initialization
	void Start () {
		childBirds = new ArrayList ();

		for (int i = 0; i < gameObject.transform.childCount; i ++) {
			Transform childBird = gameObject.transform.GetChild(i);

			Behaviour halo = childBird.GetComponent("Halo") as Behaviour;
			halo.enabled = false;

			childBirds.Add(gameObject.transform.GetChild(i));
		}
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh scoreHitCountText = scoreHitCount.GetComponent<TextMesh> ();

		int inactiveBirds = 0;
		for (int i = 0; i < childBirds.Count; i ++) {
			Transform childBird = (Transform) childBirds[i];
			if (!childBird.gameObject.GetActive())
				inactiveBirds ++;
		}

		if (inactiveBirds == childBirds.Count) 
			scoreHitCountText.text = "You Win !";
		else
			scoreHitCountText.text = inactiveBirds.ToString() + "/9";


	}
}
