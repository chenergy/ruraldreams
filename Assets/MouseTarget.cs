using UnityEngine;
using System.Collections;

public class MouseTarget : MonoBehaviour {
	public GameObject cursor;
	// Use this for initialization
	void Start () {
		this.cursor = GameObject.Instantiate(cursor, Vector3.zero, Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main != null){
			Ray target = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast (target, out hit)){
				this.cursor.transform.position = hit.point + new Vector3( 0.0f, 0.1f, 0.0f);
			}
		}
	}
}
