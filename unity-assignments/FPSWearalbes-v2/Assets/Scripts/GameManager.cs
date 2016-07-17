using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private GameObject user;
	private GameObject enemyInstance;
	private GameObject scorePanel;
	private ArrayList enemies;
	private ArrayList spawnPositions;
	private int totalNumberOfEnemies;
	private Slider userLifeline;
	private GameObject endingMessage;
	private GameObject arena;

	// Use this for initialization
	void Start () {
		enemyInstance = GameObject.Find ("Enemy");
		scorePanel = GameObject.Find ("Score Board");
		user = GameObject.Find ("User");
		arena = GameObject.Find ("Arena");

		if (Application.loadedLevelName != "maze") {
			userLifeline = GameObject.Find ("Lifeline").GetComponent<Slider> ();
			userLifeline.onValueChanged.AddListener (ListenerMethod);
		}

		endingMessage = GameObject.Find ("Ending Message");
		endingMessage.SetActive (false);

		if (Application.loadedLevelName == "snow") {
			Debug.Log ("Enemy Found");
			spawnPositions = new ArrayList ();
			enemies = new ArrayList ();

			spawnPositions.Add (new Vector3 (1f, 0f, 15f));
			spawnPositions.Add (new Vector3 (-3f, 0f, 15f));
			spawnPositions.Add (new Vector3 (-15f, 0f, 15f));
			spawnPositions.Add (new Vector3 (-15f, 0f, 7.5f));
			spawnPositions.Add (new Vector3 (-15f, 0f, 0f));
			spawnPositions.Add (new Vector3 (-7.5f, 0f, 0f));
			spawnPositions.Add (new Vector3 (0f, 0f, 0f));
			spawnPositions.Add (new Vector3 (7.5f, 0f, 0f));
			spawnPositions.Add (new Vector3 (15f, 0f, 0f));
			spawnPositions.Add (new Vector3 (15f, 0f, 7.5f));
			spawnPositions.Add (new Vector3 (15f, 0f, -7.5f));
			spawnPositions.Add (new Vector3 (0f, 0f, -7.5f));
			spawnPositions.Add (new Vector3 (-15f, 0f, -7.5f));
			spawnPositions.Add (new Vector3 (-15f, 0f, -15f));
			spawnPositions.Add (new Vector3 (15f, 0f, -15f));

			user.transform.position = new Vector3 (Random.Range (-14, 15), 1.5f, -18.0f);

			enemies.Add (enemyInstance);
			totalNumberOfEnemies = spawnPositions.Count + 1;

		}
	}

	public void ListenerMethod(float value)
	{
		if (value == userLifeline.minValue) {
			Debug.Log (" user must be dead now ! ");
			GameEnd(false);
		}
	}

	public void GameEnd(bool isVictory) {

		if (Application.loadedLevelName == "snow") {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Shootable");
			for (int index = 0; index < enemies.Length; index ++)
				enemies [index].SetActive (false);
		
			GameObject pillers = GameObject.Find ("pillers");
			GameObject boxes = GameObject.Find ("boxes");
			GameObject scoreCard = GameObject.Find ("Score Card");

			if (pillers != null)
				pillers.SetActive (false);
			if (boxes != null)
				boxes.SetActive (false);

			scoreCard.SetActive (false);
		
			endingMessage.SetActive (true);

			TextMesh endingMessageTextMesh = endingMessage.GetComponent<TextMesh> ();

			if (!isVictory) {
				endingMessageTextMesh.text = "You Lose";
				endingMessage.GetComponent<MeshRenderer> ().material.color = Color.red;
			}
			else {
				endingMessageTextMesh.text = "You Win";
				endingMessage.GetComponent<MeshRenderer> ().material.color = Color.green;
			}
		}

		if (Application.loadedLevelName == "desert") {

			GameObject drone = GameObject.Find ("PA_Drone");
			GameObject targetoutPost  = GameObject.Find ("targetoutPost ");
			GameObject zombie = GameObject.Find ("tyrant _zombie");
			GameObject outPost = GameObject.Find ("outPost");
			GameObject lerpz = GameObject.Find ("Lerpz");

			if (drone != null)
				drone.SetActive (false);
			if (targetoutPost != null)
				targetoutPost.SetActive (false);
			if (zombie != null)
				zombie.SetActive (false);
			if (outPost != null)
				outPost.SetActive (false);
			if (lerpz != null)
				lerpz.SetActive (false);

			endingMessage.SetActive (true);

			user.transform.LookAt (endingMessage.transform);

			TextMesh endingMessageTextMesh = endingMessage.GetComponent<TextMesh> ();

			if (!isVictory) {
				endingMessageTextMesh.text = "You Lose";
				endingMessage.GetComponent<MeshRenderer> ().material.color = Color.red;
			}
			else {
				endingMessageTextMesh.text = "You Win";
				endingMessage.GetComponent<MeshRenderer> ().material.color = Color.green;
			}
		}
	}

	void Update () {

		if (GameObject.FindGameObjectWithTag ("GameEnd") == null && enemyInstance != null) {
			bool needSpawning = true;
			int activeEnemies = 0;
			int battlefeildDeadEnemies = 0;

			for (int index = 0; index < enemies.Count; index ++) {
				if (((GameObject)enemies [index]).activeSelf) {
					enemyInstance = (GameObject)enemies [index];
					activeEnemies ++;
				} else {
					battlefeildDeadEnemies ++;
				}

				if (activeEnemies >= 2)
					needSpawning = false;
			}

			TextMesh scoreHitCountTextMesh = scorePanel.GetComponent<TextMesh> ();
			scoreHitCountTextMesh.text = " [ " + battlefeildDeadEnemies.ToString () + " | " + totalNumberOfEnemies.ToString () + " ] ";

			Debug.Log (" need spawning : " + needSpawning);

			if (needSpawning) {
				if (spawnPositions.Count > 0) {
					GameObject enemyGameObjectOne = Instantiate (enemyInstance, (Vector3)spawnPositions [0], enemyInstance.transform.rotation) as GameObject; 
					spawnPositions.RemoveAt (0);
					enemyGameObjectOne.name = "EnemyInstance-" + enemies.Count;
					enemies.Add (enemyGameObjectOne);
					activeEnemies ++;

					Debug.Log (" Spawning enemy : " + enemyGameObjectOne.name + " at : " + enemyGameObjectOne.transform.position);

					if (spawnPositions.Count > 0 && activeEnemies < 2) {
						GameObject enemyGameObjectTwo = Instantiate (enemyInstance, (Vector3)spawnPositions [0], enemyInstance.transform.rotation) as GameObject; 
						spawnPositions.RemoveAt (0);
						enemyGameObjectTwo.name = "EnemyInstance-" + enemies.Count;
						enemies.Add (enemyGameObjectTwo);
				
						Debug.Log (" Spawning enemy : " + enemyGameObjectTwo.name + " at : " + enemyGameObjectTwo.transform.position);
					}
				} else {
					Debug.Log (" All Dead !");
					GameEnd(true);
				}
			}
		}
	}
}
