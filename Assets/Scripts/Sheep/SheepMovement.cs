using UnityEngine;
using System.Collections;

public enum SheepState { IDLE, FOLLOWING, THROWN };

//[RequireComponent(typeof(Rigidbody))]
public class SheepMovement : MonoBehaviour
{
	public float 		followSpeed 		= 13.0f;
	public float		followDistance 		= 2.0f;
	public float		throwStartHeight 	= 1.0f;
	public float		weight				= 1.0f;
	public GameObject 	target;
	public GameObject	radius;
	public GameObject 	back;
	public GameObject	deathParts;
	
	[HideInInspector]
	public SheepState state = SheepState.IDLE;
	
	private BoxCollider	controller;
	private float throwTimer = 0.0f;
	// Use this for initialization
	void Start ()
	{
		this.controller = this.gameObject.GetComponent<BoxCollider>();
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null)
			Physics.IgnoreCollision(this.collider, player.collider);
		this.collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (GameObject sheep in GameObject.FindGameObjectsWithTag("Sheep")){
			Collider other = sheep.collider;
			if (other.tag != this.tag)
				Physics.IgnoreCollision(this.collider, other.collider);
		}
		
		switch (this.state){
		case SheepState.IDLE:
			this.rigidbody.useGravity 		= true;
			this.rigidbody.freezeRotation 	= true;
			this.collider.enabled 			= true;
			this.radius.collider.enabled	= true;
			break;
		case SheepState.FOLLOWING:
			/*
			this.rigidbody.useGravity 		= false;
			this.rigidbody.freezeRotation 	= true;
			this.collider.enabled 			= false;
			this.radius.collider.enabled	= false;
			*/
			this.rigidbody.useGravity 		= true;
			this.rigidbody.freezeRotation 	= false;
			this.collider.enabled 			= true;
			this.radius.collider.enabled	= false;
			if (this.target != null){
				if ((this.target.transform.position - this.collider.transform.position).magnitude > 0.1f){
					this.collider.transform.position = Vector3.Lerp( this.collider.transform.position, new Vector3(this.target.transform.position.x, 0.5f, this.target.transform.position.z), Time.deltaTime * this.followSpeed );
					this.collider.transform.LookAt( new Vector3(this.target.transform.position.x, 0.5f, this.target.transform.position.z) );
				}
			}
			
			//float yPosition = 0.5f;
			//this.collider.transform.position = new Vector3(this.collider.transform.position.x, yPosition, this.collider.transform.position.z);
			break;
		case SheepState.THROWN:
			this.rigidbody.useGravity 		= true;
			this.rigidbody.freezeRotation 	= false;
			this.collider.enabled 			= true;
			this.radius.collider.enabled 	= false;
			break;
		default:
			break;
		}
		
		if (this.state == SheepState.THROWN){
			this.throwTimer += Time.deltaTime;
		}
		if (this.throwTimer >= 3.0f){
			this.throwTimer = 0.0f;
			this.state = SheepState.IDLE;
		}
		
		this.ReOrient();
	}
	
	private bool IsGrounded{
		// Raycast for more accuracy than controller.isGrounded
		get{ return Physics.Raycast( new Vector3(this.controller.transform.position.x, 
				this.controller.transform.position.y - this.controller.size.y * 0.4f,
				this.controller.transform.position.z), Vector3.down, 0.2f); }
	}
	
	private void AddGravity (){
		this.controller.transform.position += Physics.gravity * Time.deltaTime;
	}
	
	private void ReOrient(){
		this.transform.rotation = Quaternion.Euler(0.0f, this.transform.rotation.eulerAngles.y, 0.0f);
	}
	
	public void SetTarget ( GameObject target ){
		this.target = target;
		this.collider.transform.position = this.target.transform.position;
	}
	
	public void Follow(GameObject target){
		this.target = target;
		this.state = SheepState.FOLLOWING;
	}
	
	public void Throw(Vector3 point){
		this.transform.position = this.target.transform.position + new Vector3(0, this.throwStartHeight, 0);
		this.rigidbody.velocity = point;
		this.state = SheepState.THROWN;
	}
}