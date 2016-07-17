using UnityEngine;
using System.Collections;

public class LightingEventHandler : MonoBehaviour {

	private GameObject directionalLight;
	private Bounds userBounds;

	// Use this for initialization
	void Start () {
		directionalLight = GameObject.Find ("Directional Light");
	}

	void Update() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Shootable");
		userBounds = gameObject.GetComponent<Collider> ().bounds;

		bool isIntersecting = false;
		for (int index = 0; index < enemies.Length; index ++) {
			Bounds enemyBounds = enemies [index].GetComponent<Collider> ().bounds;

			if (userBounds.Intersects (enemyBounds)) {
				isIntersecting = true;
				break;
			}
		}

		Light mainLight = directionalLight.GetComponent<Light> ();
		if (isIntersecting) {
			mainLight.color = Color.red;
		} else {
			mainLight.color = Color.white;
		}
	}
}
