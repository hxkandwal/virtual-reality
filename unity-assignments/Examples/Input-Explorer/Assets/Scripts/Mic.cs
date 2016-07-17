using UnityEngine;
using System.Collections;

// IMPORTANT: If external speakers are used with a built-in microphone,
// the microphone will pick up any audio feedback in addition to the
// user's voice.

public class Mic : MonoBehaviour {

	// Track the amount of time passed
	float TimePassed = 0.0f;
	
	// Need an audio source to be heard
	AudioSource Sound;

	// Use this for initialization
	void Start () {
	
		// Get the GameObject's audio source
		Sound = gameObject.GetComponent<AudioSource>();
		
		// Record for 5 seconds
		Sound.clip = Microphone.Start(null, false, 5, 44100);
		
		// Then play the clip 5 seconds later
		Sound.PlayDelayed(5.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
		// Track the amount of time passed since last frame
		TimePassed += Time.deltaTime;
		
		// If 10 seconds have passed 
		if(TimePassed > 10.0f) {
		
			// Reset our timer
			TimePassed = 0.0f;
		
			// Record another 5 seconds
			Sound.clip = Microphone.Start(null, false, 5, 44100);
			
			// Then play the clip 5 seconds later
			Sound.PlayDelayed(5.0f);
		}
	}
	
}
