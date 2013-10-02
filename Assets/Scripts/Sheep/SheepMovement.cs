using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]

public class SheepMovement : MonoBehaviour
{
	public GameObject target;
	public float followSpeed = 5.0f;
	public GameObject back;
	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null){
			Physics.IgnoreCollision(this.collider, player.collider);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.target != null){
			//this.transform.position = this.target.transform.position + this.target.transform.TransformDirection(Vector3.back);
			this.collider.transform.position = Vector3.Lerp( this.collider.transform.position, this.target.transform.position, Time.deltaTime * this.followSpeed );
			this.collider.transform.LookAt( this.target.transform.position );
		}
		
		if ( Input.GetKey(KeyCode.Q)){
			this.SetTarget( GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().back );
		}
	}
	
	public void SetTarget ( GameObject target ){
		this.target = target;
		this.collider.transform.position = this.target.transform.position;
	}
}

