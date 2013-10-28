using UnityEngine;
using System.Collections;

public class OverheadTrigger : MonoBehaviour
{
	public string message;
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			GameObject.Find("UI").GetComponent<OverheadDisplay>().text = this.message;
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag == "Player"){
			GameObject.Find("UI").GetComponent<OverheadDisplay>().text = "";
		}
	}
}

