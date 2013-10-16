using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSheepActions : MonoBehaviour
{
	public GameObject littleSheep;
	public GameObject bigSheep;
	public GameObject back;
	public GameObject top;
	
	[HideInInspector]
	public List<GameObject> sheepList;
	
	private GameObject lastBack;
	private float sheepDestructionTimer = 0.0f;
	// Use this for initialization
	void Start ()
	{
		sheepList = new List<GameObject>();
		this.lastBack = back;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			this.AddLittleSheep();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			this.AddBigSheep();
		}
		if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Joystick1Button18)){
			this.ThrowSheep();
		}
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button14)){
			this.CycleSheepBack();
		}
		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button13)){
			this.CycleSheepFront();
		}
		if (Input.GetKey(KeyCode.T)){
			this.DestroyAllSheep();
		}else{
			this.sheepDestructionTimer = 0.0f;
		}
		//this.rigidbody.mass = 1 + this.sheepList.Count;
	}
	
	public void AddLittleSheep (){
		GameObject newSheep = GameObject.Instantiate(this.littleSheep, this.back.transform.position, this.littleSheep.transform.rotation) as GameObject;
		newSheep.GetComponent<SheepMovement>().target = this.lastBack;
		newSheep.GetComponent<SheepMovement>().state = SheepState.FOLLOWING;
		this.lastBack = newSheep.GetComponent<SheepMovement>().back;
		this.sheepList.Add(newSheep);
	}
	
	public void AddBigSheep (){
		GameObject newSheep = GameObject.Instantiate(this.bigSheep, this.back.transform.position, this.bigSheep.transform.rotation) as GameObject;
		newSheep.GetComponent<SheepMovement>().target = this.lastBack;
		newSheep.GetComponent<SheepMovement>().state = SheepState.FOLLOWING;
		this.lastBack = newSheep.GetComponent<SheepMovement>().back;
		this.sheepList.Add(newSheep);
	}
	
	public void PickupSheep( GameObject sheep ){
		SheepMovement script = sheep.GetComponent<SheepMovement>();
		script.target = this.lastBack;
		script.state = SheepState.FOLLOWING;
		this.lastBack = script.back;
		this.sheepList.Add(sheep);
	}
	
	public void ThrowSheep (){
		if (this.sheepList.Count > 0){
			GameObject newSheep = new GameObject();
			if (this.sheepList[0].name == "LittleSheep(Clone)" || this.sheepList[0].name == "LittleSheep"){
				GameObject.Destroy(newSheep);
				newSheep = GameObject.Instantiate(this.littleSheep, this.top.transform.position, this.littleSheep.transform.rotation) as GameObject;
			}
			else if (this.sheepList[0].name == "BigSheep(Clone)" || this.sheepList[0].name == "BigSheep"){
				GameObject.Destroy(newSheep);
				newSheep = GameObject.Instantiate(this.bigSheep, this.top.transform.position, this.bigSheep.transform.rotation) as GameObject;
			}
			newSheep.GetComponent<SheepMovement>().state = SheepState.THROWN;
			newSheep.rigidbody.velocity = this.collider.transform.TransformDirection(new Vector3(0.0f, 5.0f, 5.0f));
			
			GameObject.Destroy(this.sheepList[0]);
			if (this.sheepList.Count > 1){
				this.sheepList[1].GetComponent<SheepMovement>().target = this.back;
			}
			else{
				this.lastBack = this.back;
			}
			this.sheepList.RemoveAt(0);
		}
	}
	
	public void CycleSheepBack(){
		if (this.sheepList.Count > 1){
			GameObject sheepFirst = this.sheepList[0];
			GameObject sheepSecond = this.sheepList[1];
			GameObject sheepLast = this.sheepList[this.sheepList.Count-1];
			
			SheepMovement scriptFirst = sheepFirst.GetComponent<SheepMovement>();
			SheepMovement scriptSecond = sheepSecond.GetComponent<SheepMovement>();
			SheepMovement scriptLast = sheepLast.GetComponent<SheepMovement>();
			
			scriptFirst.target = scriptLast.back;
			scriptSecond.target = this.back;
			this.lastBack = scriptFirst.back;
			
			this.sheepList.RemoveAt(0);
			this.sheepList.Add(sheepFirst);
		}
	}
	
	public void CycleSheepFront(){
		if (this.sheepList.Count > 1){
			GameObject sheepFirst = this.sheepList[0];
			GameObject sheepLast = this.sheepList[this.sheepList.Count-1];
			
			SheepMovement scriptFirst = sheepFirst.GetComponent<SheepMovement>();
			SheepMovement scriptLast = sheepLast.GetComponent<SheepMovement>();
			
			this.lastBack = scriptLast.target;
			scriptLast.target = this.back;
			scriptFirst.target = scriptLast.back;
			
			this.sheepList.RemoveAt(this.sheepList.Count-1);
			this.sheepList.Insert(0, sheepLast);
		}
	}
	
	private void DestroyAllSheep(){
		if (this.sheepDestructionTimer > 1.0f){
			this.sheepDestructionTimer = 0.0f;
			foreach (GameObject sheep in GameObject.FindGameObjectsWithTag("Sheep")){
				SheepMovement movement = sheep.GetComponent<SheepMovement>();
				GameObject sheepParts = GameObject.Instantiate(movement.deathParts, sheep.transform.position, Quaternion.identity) as GameObject;
				GameObject.Destroy(sheepParts, 2.0f);
				GameObject.Destroy(sheep);
			}
		}
		else{
			this.sheepDestructionTimer += Time.deltaTime;
		}
	}
}

