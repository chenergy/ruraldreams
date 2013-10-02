using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject character;
	public float zoomOutY = 2.5f;
	public float zoomOutZ = 5.0f;
	void Start(){
		if (this.character != null){
			this.transform.position = new Vector3(character.transform.position.x, 
				character.transform.position.y + this.zoomOutY, 
				character.transform.position.z - this.zoomOutZ);
			this.transform.LookAt(character.transform.position);
		}
	}
	void Update(){
		if (this.character != null){
			this.transform.position = new Vector3(character.transform.position.x, 
				character.transform.position.y + this.zoomOutY, 
				character.transform.position.z - this.zoomOutZ);
			this.transform.LookAt(character.transform.position);
		}
	}
}