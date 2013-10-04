using UnityEngine;
using System.Collections;

public class ControlsUI : MonoBehaviour {
	public GUIStyle leftJustified;
	public GUIStyle rightJustified;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		GUI.Box(new Rect(20,0,200,100), 
			"1 - Create Little Sheep\n2 - Create Big Sheep\nTab - Throw Sheep\nQ - Cycle Sheep Back\nE - Cycle Sheep To Front\nR - Open Door/Activate", 
			leftJustified);
		GUI.Box(new Rect(Screen.width - 220,0,200,100), 
			"W - Forward\nA - Left\nD - Right\nS - Back\nSpace - Jump",
			rightJustified);
	}
}
