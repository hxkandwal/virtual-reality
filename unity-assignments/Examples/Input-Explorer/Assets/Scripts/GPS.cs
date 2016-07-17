using UnityEngine;
using System.Collections;

// IMPORTANT: Location information by default is based on Wifi signal. You must disable 
// Wifi location services to force true GPS data. However, without a service provider
// signal, the GPS data will not work in Unity.

public class GPS : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
	
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			yield break;
		}
		
		// Start service before querying location
		Input.location.Start();
		
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		
		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			Debug.Log("Timed out");
			yield break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		// Connection failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.Log("Unable to determine device location");
		}
		// Location services is working
		else {
			// Output latitude, longitude, and altitude values as 3D text
			GameObject.Find("Latitude Value").GetComponent<TextMesh>().text = Input.location.lastData.latitude.ToString();
			GameObject.Find("Longitude Value").GetComponent<TextMesh>().text = Input.location.lastData.longitude.ToString();
			GameObject.Find("Altitude Value").GetComponent<TextMesh>().text = Input.location.lastData.altitude.ToString();
		}
	}
	
}
