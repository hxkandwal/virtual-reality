using UnityEngine;
using System.Collections;

public class AnimateSprite : MonoBehaviour {

	private long flipFrequency;

	public Sprite image1;
	public Sprite image2;
	public Sprite image3;

	// Update is called once per frame
	void Update () {
		flipFrequency ++;

		if (flipFrequency % 3 == 1) {

			gameObject.GetComponent<SpriteRenderer> ().sprite = image1;
		}
		else if (flipFrequency % 3 == 2) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = image2;
		}
		else {
			gameObject.GetComponent<SpriteRenderer> ().sprite = image3;
		}
	}

	void OnTriggerStay() {
		Debug.Log (" jaloing Halo");
		Behaviour halo = gameObject.GetComponent("Halo") as Behaviour;
		halo.enabled = true;
	}

	void OnTriggerExit() {
		Debug.Log (" Bujhaoing Halo");
		Behaviour halo = gameObject.GetComponent("Halo") as Behaviour;
		halo.enabled = false;
	}

	void OnTriggerEnter() {
		Debug.Log (" Trigger Enter");
		Behaviour halo = gameObject.GetComponent("Halo") as Behaviour;
		halo.enabled = false;
	}
}
