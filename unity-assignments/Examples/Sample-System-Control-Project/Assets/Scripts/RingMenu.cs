using UnityEngine;
using System.Collections;

public class RingMenu : MonoBehaviour {

	// Gameobjects related to the ring menu
	GameObject Menu, Command1, Command2, Command3, Command4, Command5, SelectionBox;
	
	// Boolean for showing the ring menu
	bool shown = false;
	
	// Boolean that command is valid
	bool valid = false;

	// Use this for initialization
	void Start () {
	
		// Get transform references to the ring menu, all five commands, and the selection box
		Menu = GameObject.Find("Ring Menu");
		Command1 = GameObject.Find("Command 1");
		Command2 = GameObject.Find("Command 2");
		Command3 = GameObject.Find("Command 3");
		Command4 = GameObject.Find("Command 4");
		Command5 = GameObject.Find("Command 5");
		SelectionBox = GameObject.Find("Selection Box");
	}
	
	// Update is called once per frame
	void Update () {
	
		// User can bring up ring menu by pressing the touchpad
		if(!shown && Input.GetMouseButton(0)) {
		
			// Keep track of modes
			shown = true;
			
			// Shown the hidden parts of the menu
			Command1.GetComponent<MeshRenderer>().enabled = true;
			Command2.GetComponent<MeshRenderer>().enabled = true;
			Command3.GetComponent<MeshRenderer>().enabled = true;
			Command4.GetComponent<MeshRenderer>().enabled = true;
			Command5.GetComponent<MeshRenderer>().enabled = true;
			SelectionBox.GetComponent<MeshRenderer>().enabled = true;
		}
		// Give the user a chance to release the touchpad without affecting the menu
		else if(shown && !valid && !Input.GetMouseButton(0)) {
			// Touchpad interactions are now valid
			valid = true;
		}
		// If the menu is shown and the touchpad was initially released
		else if(shown && valid) {
		
			// Determine how many degrees the menu is from one of its five positions
			// (i.e., 360 degrees / 5 = 72 degrees)
			float degreesTill = Menu.transform.localEulerAngles.z % 72.0f;
			
			// Rotate the menu 72 degrees per second if the touchpad is pressed
			// or if the menu is more than 6 degrees from its next position
			if(Input.GetMouseButton(0) || degreesTill >= 6.0f) {
				Menu.transform.Rotate(new Vector3(0.0f, 0.0f, -72.0f * Time.deltaTime));
			}
			// Rotate the menu to its next position if the touchpad is not pressed
			else if(degreesTill <= 6.0f) {
				Menu.transform.Rotate(new Vector3(0.0f, 0.0f, -degreesTill));
			}	
			
			// Rotate every command panel to its original orientation
			Command1.transform.rotation = SelectionBox.transform.rotation;
			Command2.transform.rotation = SelectionBox.transform.rotation;
			Command3.transform.rotation = SelectionBox.transform.rotation;
			Command4.transform.rotation = SelectionBox.transform.rotation;
			Command5.transform.rotation = SelectionBox.transform.rotation;
			
			// Check if the back button is pressed and command is valid
			if(Input.GetMouseButton(1) && valid) {
			
				// Determine which command is being selected and call the appropriate function
				if(Vector3.Distance(SelectionBox.transform.position, Command1.transform.position) < 0.05f) {
					ExecuteCommand1();
				}
				else if(Vector3.Distance(SelectionBox.transform.position, Command2.transform.position) < 0.05f) {
					ExecuteCommand2();
				}
				else if(Vector3.Distance(SelectionBox.transform.position, Command3.transform.position) < 0.05f) {
					ExecuteCommand3();
				}
				else if(Vector3.Distance(SelectionBox.transform.position, Command4.transform.position) < 0.05f) {
					ExecuteCommand4();
				}
				else if(Vector3.Distance(SelectionBox.transform.position, Command5.transform.position) < 0.05f) {
					ExecuteCommand5();
				}
			}
		}
	}
	
	// Execute functionality for first command
	void ExecuteCommand1 () {
		// Load the scene named "Abstract"
		Application.LoadLevel("Abstract");
	}
	
	// Execute functionality for second command
	void ExecuteCommand2 () {
		// Load the scene named "Desert"
		Application.LoadLevel("Desert");
	}
	
	// Execute functionality for third command
	void ExecuteCommand3 () {
		// Load the scene named "Dungeon"
		Application.LoadLevel("Dungeon");
	}
	
	// Execute functionality for fourth command
	void ExecuteCommand4 () {
		// Load the scene named "Table"
		Application.LoadLevel("Table");
	}
	
	// Execute functionality for fifth command
	void ExecuteCommand5 () {
		// Load the scene named "Valley"
		Application.LoadLevel("Valley");
	}
	
	
}
