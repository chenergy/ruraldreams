using UnityEngine;
using System.Collections;

public class OverheadDisplay : MonoBehaviour
{
	public string 	text;
	public float 	speed = 1.0f;
	public GUIStyle style;
	
	private int 	nextCharInput = 0;
	private float	timer;
	private string	finalText;
	
	void Start(){
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (timer >= 0.01f/this.speed){
			timer = 0.0f;
			if (this.text.Length > 0){
				if (this.finalText != this.text){
					this.finalText += text[this.nextCharInput];
					this.nextCharInput++;
				}
			}
			else{
				this.finalText = "";
				this.nextCharInput = 0;
			}
		}
		else{
			timer += Time.deltaTime;
		}
	}
	
	void OnGUI(){
		GUI.Box(new Rect(Screen.width/2.0f - 200, Screen.height/2.0f - 200, 400, 200), this.finalText, style);
	}
}

