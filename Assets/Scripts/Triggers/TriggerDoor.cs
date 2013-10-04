using UnityEngine;
using System.Collections;

public class TriggerDoor : MonoBehaviour {
	public GameObject left;
	public GameObject right;
	public GameObject wall;
	
	private bool open = false;
	private Quaternion leftTargetRotation;
	private Quaternion rightTargetRotation;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float difference = (left.transform.rotation.eulerAngles - leftTargetRotation.eulerAngles).magnitude;
		if (difference < 0.1f){
			left.transform.rotation = leftTargetRotation;
			right.transform.rotation = rightTargetRotation;
		}
		else{
			left.transform.rotation = Quaternion.Slerp(left.transform.rotation, leftTargetRotation, Time.deltaTime * 3.0f);
			right.transform.rotation = Quaternion.Slerp(right.transform.rotation, rightTargetRotation, Time.deltaTime * 3.0f);
		}
	}
	
	void OnTriggerStay(Collider other){
		if (Input.GetKeyDown(KeyCode.R)){
			if (other.tag == "Player"){
				if (!this.open){
					this.leftTargetRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
					this.rightTargetRotation = Quaternion.Euler( 0.0f, -90.0f, 0.0f );
					this.wall.collider.enabled = false;
					this.open = true;
				}
				else{
					this.leftTargetRotation = Quaternion.Euler( 0.0f, 0.0f, 0.0f );
					this.rightTargetRotation = Quaternion.Euler( 0.0f, 0.0f, 0.0f );
					this.wall.collider.enabled = true;
					this.open = false;
				}
			}
		}
	}
}
