using UnityEngine;
using System.Collections;

public class ParticleTrigger : MonoBehaviour {
	public GameObject particle;
	
	void Start(){
		this.particle.renderer.enabled = false;
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			this.particle.renderer.enabled = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag == "Player"){
			this.particle.renderer.enabled = false;
		}
	}
}
