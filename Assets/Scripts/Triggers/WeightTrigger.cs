using UnityEngine;
using System.Collections;

public class WeightTrigger : MonoBehaviour
{
	public float maxWeight = 1.0f;
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Sheep"){
			
		}
	}
}

