using UnityEngine;
using System.Collections;

public class TextTrigger : MonoBehaviour
{
	public string message;
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			GameObject.Find("UI").GetComponent<TextDisplay>().text = this.message;
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag == "Player"){
			GameObject.Find("UI").GetComponent<TextDisplay>().text = "";
		}
	}
}

