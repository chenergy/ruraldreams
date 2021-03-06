using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(CharacterController))]

public class BehindTheBackMovement : MonoBehaviour
{
	public GameObject			mesh;
	public float 				moveSpeed 		= 1.0f;
	public float 				jumpStrength 	= 10.0f;
	
	public AudioClip			walkingSound;
	public AudioClip			jumpingSound;
	
	private Vector3 			movement;
	private Quaternion 			rotation;
	private bool 				jumping 		= false;
	//private CharacterController controller;
	private CapsuleCollider		controller;
	private Quaternion			targetRotation;
	// Use this for initialization
	void Start ()
	{
		//this.controller = this.gameObject.GetComponent<CharacterController>();
		this.controller = this.gameObject.GetComponent<CapsuleCollider>();
		this.movement 	= Vector3.zero;
		Camera.main.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((Mathf.Abs(Input.GetAxis("HorizontalKey")) > 0) || 
			(Mathf.Abs(Input.GetAxis("HorizontalJoystick")) > 0) || 
			(Mathf.Abs(Input.GetAxis("VerticalKey")) > 0) || 
			(Mathf.Abs(Input.GetAxis("VerticalJoystick")) > 0)){
			
			if ( Input.GetAxis("VerticalKey") > 0 || Input.GetAxis("VerticalJoystick") > 0){
				//this.controller.transform.position += new Vector3(0.0f, 0.0f, (Input.GetAxis("VerticalKey") + Input.GetAxis("VerticalJoystick")) * 0.5f * Time.deltaTime * this.moveSpeed);
				this.controller.transform.position += this.controller.transform.TransformDirection( new Vector3(0.0f, 0.0f, (Input.GetAxis("VerticalKey") + Input.GetAxis("VerticalJoystick")) * Time.deltaTime * this.moveSpeed) );
			}
			// Down
			if ( Input.GetAxis("VerticalKey") < 0 || Input.GetAxis("VerticalJoystick") < 0){
				//this.controller.transform.position += new Vector3(0.0f, 0.0f, (Input.GetAxis("VerticalKey") + Input.GetAxis("VerticalJoystick")) * 0.5f * Time.deltaTime * this.moveSpeed);
				this.controller.transform.position += this.controller.transform.TransformDirection( new Vector3(0.0f, 0.0f, (Input.GetAxis("VerticalKey") + Input.GetAxis("VerticalJoystick")) * Time.deltaTime * this.moveSpeed) );
			}
			// Right
			if ( Input.GetAxis("HorizontalKey") > 0 || Input.GetAxis("HorizontalJoystick") > 0){
				this.controller.transform.Rotate( new Vector3( 0.0f, (Input.GetAxis("HorizontalKey") + Input.GetAxis("HorizontalJoystick")) * Time.deltaTime * 10.0f * this.moveSpeed, 0.0f) );
				//this.controller.transform.position += new Vector3((Input.GetAxis("HorizontalKey") + Input.GetAxis("HorizontalJoystick")) * 0.5f * Time.deltaTime * this.moveSpeed, 0.0f, 0.0f );
			}
			// Left
			if ( Input.GetAxis("HorizontalKey") < 0 || Input.GetAxis("HorizontalJoystick") < 0){
				this.controller.transform.Rotate( new Vector3( 0.0f, (Input.GetAxis("HorizontalKey") + Input.GetAxis("HorizontalJoystick")) * Time.deltaTime * 10.0f * this.moveSpeed, 0.0f) );
				//this.controller.transform.position += new Vector3((Input.GetAxis("HorizontalKey") + Input.GetAxis("HorizontalJoystick")) * 0.5f * Time.deltaTime * this.moveSpeed, 0.0f, 0.0f );
			}
		
			this.mesh.animation.CrossFade("farmer_walk");
			//this.collider.transform.rotation = this.rotation;
		}
		else{
			this.GetComponent<AudioSource>().clip = null;
			this.GetComponent<AudioSource>().enabled = false;
			this.mesh.animation.CrossFade("farmer_idle");
		}
		
		// Jump
		if (!this.jumping){
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button16)){
				this.Jump();
			}
			this.GetComponent<AudioSource>().clip = this.walkingSound;
			this.GetComponent<AudioSource>().enabled = true;
		}
		else{
			this.GetComponent<AudioSource>().enabled = false;
		}
		
		// Spherically interporlate towards a target rotation
		/*
		Vector3 lookDirection = new Vector3( Input.GetAxis("HorizontalKey") + Input.GetAxis("HorizontalJoystick") * 0.5f,
			0.0f, 
			Input.GetAxis("VerticalKey") + Input.GetAxis("VerticalJoystick") * 0.5f);
		*/
		//this.rotation = Quaternion.Slerp( this.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 10.0f);
		//this.controller.transform.rotation = Quaternion.Slerp( this.controller.transform.rotation, this.targetRotation, Time.deltaTime * 10.0f);
		/*
		if (Input.GetAxis("HorizontalKey") 		!= 0.0f ||
			Input.GetAxis("HorizontalJoystick") != 0.0f ||
			Input.GetAxis("VerticalKey") 		!= 0.0f ||
			Input.GetAxis("VerticalJoystick") 	!= 0.0f){
			this.collider.transform.rotation = this.rotation;
		}
		*/
		this.controller.transform.position += this.movement * Time.deltaTime;
		//this.controller.Move( this.movement * Time.deltaTime );
		
		// Apply gravity if not on the ground
		/*
		if (!this.IsGrounded){
			this.AddGravity();
		}
		else{
			this.movement.y = 0.0f;
			this.jumping = false;
		}
		*/
		if (this.IsGrounded){ 
			this.jumping = false;
		}
		else {
			this.mesh.animation.CrossFade("farmer_jump");
			this.jumping = true; 
		}
		
		//Debug.Log("Grounded? " + this.IsGrounded.ToString() );
		Debug.DrawRay(new Vector3(this.controller.transform.position.x, 
				this.controller.transform.position.y - this.controller.height * 0.5f,
				this.controller.transform.position.z), Vector3.down * 0.2f);
	}
	
	private bool IsGrounded{
		// Raycast for more accuracy than controller.isGrounded
		/*get{ return Physics.Raycast( new Vector3(this.controller.transform.position.x, 
				this.controller.transform.position.y - this.controller.height * 0.5f,
				this.controller.transform.position.z), Vector3.down, 0.2f); }
				*/
		get{ return Physics.Raycast( new Vector3(this.controller.transform.position.x, 
				this.controller.transform.position.y - this.controller.height * 0.4f,
				this.controller.transform.position.z), Vector3.down, 0.2f); }
	}
	
	private void AddGravity (){
		this.movement += Physics.gravity * Time.deltaTime;
	}
	
	private void Jump(){
		AudioSource.PlayClipAtPoint(this.jumpingSound, Camera.main.transform.position, 0.5f);
		this.jumping = true;
		/*
		this.movement.y += this.jumpStrength;
		*/
		this.rigidbody.AddForce (this.jumpStrength * Vector3.up * 40.0f * this.rigidbody.mass);
	}
}