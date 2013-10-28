using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {
	public SpawnType type = SpawnType.SHEEP;
	
	[HideInInspector]
	public GameObject target;
	
	// Use this for initialization
	void Start () {
		renderer.enabled = false;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
