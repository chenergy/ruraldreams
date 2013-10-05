using UnityEngine;
using System.Collections;

public enum SheepState { IDLE, FOLLOWING, THROWN };

//[RequireComponent(typeof(Rigidbody))]
public class SheepMovement : MonoBehaviour
{
	public float 		followSpeed = 7.0f;
	public GameObject 	target;
	public GameObject 	back;
	public GameObject	radius;
	
	[HideInInspector]
	public SheepState state = SheepState.IDLE;
	
	private float throwTimer = 0.0f;
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
		switch (this.state){
		case SheepState.IDLE:
			this.rigidbody.velocity 		= Vector3.zero;
			this.rigidbody.useGravity 		= false;
			this.transform.rotation 		= Quaternion.identity;
			this.collider.enabled 			= false;
			this.radius.collider.enabled 	= true;
			this.transform.position 		= new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
			break;
		case SheepState.FOLLOWING:
			this.rigidbody.useGravity 		= false;
			this.rigidbody.freezeRotation 	= true;
			this.collider.enabled 			= false;
			this.radius.collider.enabled	= false;
			if (this.target != null){
				if ((this.target.transform.position - this.collider.transform.position).magnitude > 0.1f){
					this.collider.transform.position = Vector3.Lerp( this.collider.transform.position, new Vector3(this.target.transform.position.x, 0.5f, this.target.transform.position.z), Time.deltaTime * this.followSpeed );
					this.collider.transform.LookAt( new Vector3(this.target.transform.position.x, 0.5f, this.target.transform.position.z) );
				}
			}
			this.collider.transform.position = new Vector3(this.collider.transform.position.x, 0.5f, this.collider.transform.position.z);
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
	}
	
	public void SetTarget ( GameObject target ){
		this.target = target;
		this.collider.transform.position = this.target.transform.position;
	}
}

