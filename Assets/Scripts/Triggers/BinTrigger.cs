using UnityEngine;
using System.Collections;

public class BinTrigger : MonoBehaviour
{
	private int sheepCount;
	public int threshold = 1;
	private bool triggered = false;

	// Use this for initialization
	void Start ()
	{
		sheepCount = 0;	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (sheepCount >= threshold && !triggered) {
			transform.root.gameObject.GetComponent<NewLevelScript> ().NextLevel ();
			sheepCount = 0;
			triggered = true;
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (triggered) {
			if (other.tag == "Sheep"){
				GameObject newDeathParts = GameObject.Instantiate(other.GetComponent<SheepMovement>().deathParts, other.gameObject.transform.position, Quaternion.identity) as GameObject;
				GameObject.Destroy(newDeathParts, 1.0f);
				Destroy (other.gameObject);
			}
		} else if (other.gameObject.tag == "Sheep") {
			sheepCount++;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Sheep") {
			sheepCount--;
		}
	}
}
