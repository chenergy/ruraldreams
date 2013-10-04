using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class SheepMovement : MonoBehaviour
{
	public GameObject target;
	public float followSpeed = 7.0f;
	public GameObject back;
	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null){
			Physics.IgnoreCollision(this.collider, player.collider);
		}
		this.collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*
		foreach ( GameObject gobj in GameObject.FindGameObjectsWithTag("Sheep")){
			if (gobj != this.gameObject){
				Physics.IgnoreCollision(gobj.collider, this.collider);
			}
		}
		*/
		
		if (this.target != null){
			if ((this.target.transform.position - this.collider.transform.position).magnitude > 0.1f){
				//this.transform.position = this.target.transform.position + this.target.transform.TransformDirection(Vector3.back);
				this.collider.transform.position = Vector3.Lerp( this.collider.transform.position, new Vector3(this.target.transform.position.x, 0.5f, this.target.transform.position.z), Time.deltaTime * this.followSpeed );
				this.collider.transform.LookAt( new Vector3(this.target.transform.position.x, 0.5f, this.target.transform.position.z) );
			}
		}
		
		this.collider.transform.position = new Vector3(this.collider.transform.position.x, 0.5f, this.collider.transform.position.z);
	}
	
	public void SetTarget ( GameObject target ){
		this.target = target;
		this.collider.transform.position = this.target.transform.position;
	}
}

