using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfBehavior : MonoBehaviour
{
	//WolfState used for determining movement.
	//TargetType used for picking priority.
	public enum WolfState
	{
		Idle,
		Chasing,
		Eating,
		Leashing
	}

	public enum TargetType
	{
		LoosePig,
		LooseSheep,
		Pig,
		Sheep,
		Inedible
	}
	
	//Store the Components of the Wolf Object
	public GameObject wolf;
	public GameObject agroTrigger;
	public GameObject leashTrigger;
	
	//Store the Attributes of the Wolf
	public float agroRange = 4.5f;
	public float leashRange = 5.0f;
	public float runSpeed = 5.0f;
	public float eatTime = 2.0f;
	public GameObject target;
	public AudioClip growl;
	
	//Internal attributes of the wolf.
	private WolfState state = WolfState.Idle;
	private List<GameObject> loosePigs = new List<GameObject> ();
	private List<GameObject> looseSheep = new List<GameObject> ();
	private List<GameObject> pigs = new List<GameObject> ();
	private List<GameObject> sheep = new List<GameObject> ();
	private List<GameObject>[] possibleTargets = new List<GameObject>[4];
	private float eatingTimer = 0.0f;
	private int targetCount = 0;
		

	// Use this for initialization
	void Start ()
	{
		this.agroTrigger.GetComponent<SphereCollider> ().radius = agroRange;
		this.leashTrigger.GetComponent<SphereCollider> ().radius = leashRange;
		possibleTargets [0] = loosePigs;
		possibleTargets [1] = looseSheep;
		possibleTargets [2] = pigs;
		possibleTargets [3] = sheep;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Picks a target off the front of the target list and chases it.
		if (this.targetCount > 0 && this.state != WolfState.Eating) {
			this.target = FindTarget ();
			if (this.target != null){
				if (this.state != WolfState.Chasing){
					AudioSource.PlayClipAtPoint(this.growl, Camera.main.transform.position, 0.1f);
					this.state = WolfState.Chasing;
				}else{
					this.state = WolfState.Chasing;
				}
			}
		}
		if (this.targetCount <= 0) {
			this.target = null;
			this.state = WolfState.Leashing;
		}
		switch (state) {
		case WolfState.Idle:
			this.wolf.animation.CrossFade("idle");
			this.wolf.rigidbody.transform.position = this.transform.position;
			this.wolf.rigidbody.transform.LookAt (Camera.main.transform.position);
			break;
		case WolfState.Chasing:
			this.wolf.animation.CrossFade("run");
			if (this.target != null){
				this.wolf.rigidbody.transform.position = Vector3.Lerp (this.wolf.rigidbody.transform.position, new Vector3(this.target.transform.position.x, this.wolf.rigidbody.transform.position.y, this.target.transform.position.z), Time.deltaTime * this.runSpeed);
				this.wolf.rigidbody.transform.LookAt (new Vector3(this.target.transform.position.x, this.wolf.rigidbody.transform.position.y, this.target.transform.position.z));
				Debug.Log((this.wolf.transform.position - this.target.transform.position).magnitude);
				if ( (this.wolf.transform.position - this.target.transform.position).magnitude < 2.0f){
					this.state = WolfState.Eating;
				}
			}
			break;
		case WolfState.Eating:
			this.wolf.animation.CrossFade("eating");
			//If Wolf is done eating set it to Leash
			if (eatingTimer >= eatTime) {
				if (this.target != null){
					GameObject newDeathParts = GameObject.Instantiate(this.target.GetComponent<SheepMovement>().deathParts, this.target.transform.position, Quaternion.identity) as GameObject;
					GameObject.Destroy(newDeathParts, 1.0f);
					GameObject.Destroy(this.target);
					GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheepActions>().ResetSheepTargets();
					this.RemoveTarget(this.target);
					this.target = null;
					this.state = WolfState.Leashing;
					this.eatingTimer = 0.0f;
				}
				break;
			}
			this.eatingTimer += Time.deltaTime;
			break;
		case WolfState.Leashing:
			//If the distance from wolfObject to Center is <0.1 set wolf state to idle
			//Else move wolfObject closer to center.
			if ((this.wolf.transform.position - this.transform.position).magnitude > 0.1f) {
				this.wolf.rigidbody.transform.position = Vector3.Lerp (this.wolf.rigidbody.transform.position, new Vector3(this.transform.position.x, this.wolf.rigidbody.transform.position.y, this.transform.position.z), Time.deltaTime * this.runSpeed);
				this.wolf.rigidbody.transform.LookAt (new Vector3(this.transform.position.x, this.wolf.rigidbody.transform.position.y, this.transform.position.z));
			} else {
				this.state = WolfState.Idle;
			}
			break;
		}
	
	}
	
	public void AddTarget (GameObject t)
	{
		//Add t to the list after determining the type of game object it is.
		if (GetType (t) != TargetType.Inedible) {
			this.possibleTargets [(int)GetType (t)].Add (t);
			this.targetCount++;
		}
	}
	
	public void RemoveTarget (GameObject t)
	{
		this.possibleTargets [(int)GetType (t)].Remove (t);
		this.targetCount--;
	}
	
	//Gets the type of the Objects based off its Tag
	TargetType GetType (GameObject t)
	{
		if (t.tag == "Pig" || t.tag == "Sheep") {
			if (t.GetComponent<SheepMovement> ().target) {
				if (t.tag == "Pig") {
					return TargetType.LoosePig;
				} else if (t.tag == "Sheep") {
					return TargetType.LooseSheep;
				} else {
					return TargetType.Inedible;
				}
			} else {
				if (t.tag == "Pig") {
					return TargetType.Pig;
				} else if (t.tag == "Sheep") {
					return TargetType.Sheep;
				} else {
					return TargetType.Inedible;
				}
			}
		} else {
			return TargetType.Inedible;
		}
	}
	//Function to return the priority target
	GameObject FindTarget ()
	{
		foreach (List<GameObject> targetList in this.possibleTargets) {
			if (targetList.Count > 0) {
				GameObject priority = targetList [0];
				float range = Range (this.wolf, targetList [0]);
				foreach (GameObject t in targetList) {
					if (range < Range (this.wolf, t)) {
						priority = t;
						range = Range (this.wolf, t);
					}
				}
				return priority;
			}
		}
		return null;
	}
	//Function returns the distance between source s and target t
	float Range (GameObject s, GameObject t)
	{
		if (s != null && t != null)
			return (t.transform.position - s.transform.position).magnitude;
		return 1000f;
	}
}

