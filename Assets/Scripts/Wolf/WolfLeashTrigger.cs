using UnityEngine;
using System.Collections;

public class WolfLeashTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag == "Sheep" || other.tag == "Pig"){
			this.collider.transform.root.gameObject.GetComponent<WolfBehavior>().RemoveTarget(other.gameObject);
		}
	}
}
