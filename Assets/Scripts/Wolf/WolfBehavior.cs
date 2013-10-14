using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfBehavior : MonoBehaviour {
	public enum WolfState{ Idle, Chasing, Eating, Leashing}	
	public enum TargetType{ LoosePig, LooseSheep, Pig, Sheep}
	
	public GameObject Wolf;
	public GameObject AgroTrigger;
	public GameObject LeashTrigger;
	
	public float AgroRange = 4.5f;
	public float LeashRange = 5.0f;
	public float RunSpeed = 5.0f;
	
	public GameObject target;
	private WolfState state = WolfState.Idle;
	private TargetType targetType;
	private SortedList<TargetType, GameObject> possibleTargets;
		

	// Use this for initialization
	void Start () {
		this.AgroTrigger.GetComponent<SphereCollider>().radius = AgroRange;
		this.LeashTrigger.GetComponent<SphereCollider>().radius = LeashRange;
		this.possibleTargets = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(state){
		case WolfState.Idle:
			break;
		case WolfState.Chasing:
			break;
		case WolfState.Eating:
			break;
		case WolfState.Leashing:
			break;
		}
	
	}
	void Attack( GameObject t){
		this.target = t;
		this.targetType = TargetType.LooseSheep;
		this.state = WolfState.Chasing;
	}
	void Leash(){
		this.state = WolfState.Leashing;
		
	}
}
