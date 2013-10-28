using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour {
	public GameObject splash;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" || other.gameObject.tag == "Sheep" || other.tag == "Wolf"){
			GameObject newSplash = GameObject.Instantiate(this.splash, other.gameObject.transform.position, this.splash.transform.rotation) as GameObject;
			GameObject.Destroy(newSplash, 2.0f);
			if (other.tag != "Player") GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheepActions>().ResetSheepTargets();
			Destroy(other.gameObject);
		}
		/*if(other.gameObject.tag == "Sheep"){
			this.transform.root.gameObject.GetComponent<LevelScript>().SpawnSheep();
		}
		else if(other.gameObject.tag == "Player"){
			this.transform.root.gameObject.GetComponent<LevelScript>().SpawnCharacter();
		}*/
	}
}
