using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformMover : MonoBehaviour
{
	public Vector3 direction = new Vector3 ();
	public float period = 1.0f;
	private float timer = 0.0f;
	//private Vector3 startPosition;
	
	//public bool walkable = true;
	//private List<GameObject> onPlatform;

	// Use this for initialization
	void Start ()
	{
		//startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;
		rigidbody.transform.Translate (direction * Mathf.Cos (2 * Mathf.PI * timer / period) * Time.deltaTime);
	}
	
	/*void OnCollisionEnter(Collision collision){
		if(walkable){
			
		}
	}
	void OnCollisionExit(Collision collision){
		
	}*/
}
