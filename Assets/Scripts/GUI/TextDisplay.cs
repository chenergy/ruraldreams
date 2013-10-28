using UnityEngine;
using System.Collections;

public class TextDisplay : MonoBehaviour
{
	public string 	text;
	public float 	speed = 1.0f;
	public GUIStyle style;
	
	private int 	nextCharInput = 0;
	private float	timer;
	private string	finalText;
	
	void Start(){
		this.finalText = "";
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
		if (this.finalText.Length > 0){
			GUI.Label(new Rect(Screen.width/2.0f - 300, Screen.height - 100, 600, 90), " ");
			GUI.TextField(new Rect(Screen.width/2.0f - 300, Screen.height - 100, 600, 90), this.finalText/*, this.style*/);
		}
	}
}

