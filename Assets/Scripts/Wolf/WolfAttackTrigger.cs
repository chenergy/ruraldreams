using UnityEngine;
using System.Collections;

public class WolfAttackTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Sheep" || other.tag == "Pig"){
			this.collider.transform.root.gameObject.GetComponent<WolfBehavior>().AddTarget(other)
		}
	}
}
