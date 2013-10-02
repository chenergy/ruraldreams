using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class CharacterMovement : MonoBehaviour
{
	public float moveSpeed = 1.0f;
	public float jumpStrength = 10.0f;
	public GameObject back;
	
	private Vector3 movement;
	private Quaternion rotation;
	
	private CharacterController controller;
	private bool jumping = false;
	
	// Use this for initialization
	void Start ()
	{
		this.controller = this.gameObject.GetComponent<CharacterController>();
		this.movement = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( Input.GetAxis("VerticalKey") > 0 || Input.GetAxis("VerticalJoystick") > 0){
			//this.controller.transform.position += Vector3.forward * Time.deltaTime * this.moveSpeed;
			this.controller.Move(Vector3.forward * Time.deltaTime * this.moveSpeed);
		}
		// Down
		if ( Input.GetAxis("VerticalKey") < 0 || Input.GetAxis("VerticalJoystick") < 0){
			this.controller.Move(Vector3.back * Time.deltaTime * this.moveSpeed);
		}
		// Right
		if ( Input.GetAxis("HorizontalKey") > 0 || Input.GetAxis("HorizontalJoystick") > 0){
			this.controller.Move(Vector3.right * Time.deltaTime * this.moveSpeed);
		}
		// Left
		if ( Input.GetAxis("HorizontalKey") < 0 || Input.GetAxis("HorizontalJoystick") < 0){
			this.controller.Move(Vector3.left * Time.deltaTime * this.moveSpeed);
		}
		// Jump
		if (!this.jumping){
			if (Input.GetKeyDown(KeyCode.Space)){
				this.Jump();
			}
		}
		
		Vector3 lookDirection = new Vector3( Input.GetAxis("HorizontalKey") + Input.GetAxis("HorizontalJoystick"),
			0.0f, 
			Input.GetAxis("VerticalKey") + Input.GetAxis("VerticalJoystick") );
		this.rotation = Quaternion.Slerp( this.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 10.0f);
		
		if (Input.anyKey){
			this.controller.transform.rotation = this.rotation;
		}
		//this.controller.transform.LookAt( this.gameObject.transform.position + lookDirection);
		
		if (!this.controller.isGrounded){
			//this.controller.transform.Translate( this.movement * Time.deltaTime );
			this.AddGravity();
		}
		else{
			this.movement.y = 0.0f;
			this.jumping = false;
		}
		this.controller.Move( this.movement * Time.deltaTime );
		Debug.Log( lookDirection );
	}
	
	private void AddGravity (){
		this.movement += Physics.gravity * Time.deltaTime;
	}
	
	private void Jump(){
		this.jumping = true;
		this.movement.y += this.jumpStrength;
	}
}

