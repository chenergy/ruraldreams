using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSheepActions : MonoBehaviour
{
	public float		throwForce = 5.0f;
	public bool			throwToPoint = false;
	public GameObject 	littleSheep;
	public GameObject 	bigSheep;
	public GameObject 	back;
	public GameObject 	top;
	
	[HideInInspector]
	public List<GameObject> sheepList;
	
	private GameObject 		lastBack;
	private float 			sheepDestructionTimer = 0.0f;
	// Use this for initialization
	void Start ()
	{
		sheepList = new List<GameObject>();
		this.lastBack = this.back;
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
		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown(KeyCode.Joystick1Button18)){
			Ray target = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast (target, out hit)){
				this.ThrowSheep(hit.point);			
			}
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
	}
	/*
	public void AddLittleSheep (){
		GameObject newSheep = GameObject.Instantiate(this.littleSheep, this.transform.position, this.littleSheep.transform.rotation) as GameObject;
		if(this.sheepList.Count == 0){
			newSheep.GetComponent<SheepMovement>().Follow(this.gameObject);
		}
		else{
			newSheep.GetComponent<SheepMovement>().Follow(this.sheepList[this.sheepList.Count -1]);
		}
		this.sheepList.Add(newSheep);
	}
	
	public void AddBigSheep (){
		GameObject newSheep = GameObject.Instantiate(this.bigSheep, this.transform.position, this.bigSheep.transform.rotation) as GameObject;
		if(this.sheepList.Count == 0){
			newSheep.GetComponent<SheepMovement>().Follow(this.gameObject);
		}
		else{
			newSheep.GetComponent<SheepMovement>().Follow(this.sheepList[this.sheepList.Count -1]);
		}
		this.sheepList.Add(newSheep);
	}
	
	public void PickupSheep( GameObject sheep ){
		if(this.sheepList.Count == 0){
			sheep.GetComponent<SheepMovement>().Follow(this.gameObject);
		}
		else{
			sheep.GetComponent<SheepMovement>().Follow(this.sheepList[this.sheepList.Count -1]);
		}
		this.sheepList.Add(sheep);
	}
	*/
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
	
	public void ThrowSheep (Vector3 point){
		if (this.sheepList.Count > 0){
			//Sets the change in X, Y, Z and Magnitute of vector from source to target
			float deltaX = point.x-this.transform.position.x;
			float deltaY = point.y-this.transform.position.y;
			float deltaZ = point.z-this.transform.position.z;
			float xzMag = Mathf.Sqrt (Mathf.Pow (deltaX, 2) + Mathf.Pow (deltaZ, 2));
			
			//Set the throw vector so that the sheep will land at the point
			if( throwToPoint ){
				float angle = (Mathf.PI/2 + Mathf.Atan2( deltaY, xzMag))/2;
				Vector3 throwVector = new Vector3(deltaX,
												  xzMag*Mathf.Tan (angle),
												  deltaZ);
				float velocity = Mathf.Sqrt ((-(Physics.gravity.y) * Mathf.Pow (xzMag, 2))/(xzMag*Mathf.Sin (2*angle)-deltaY*Mathf.Cos (2*angle)-deltaY));
				this.sheepList[0].GetComponent<SheepMovement>().Throw (throwVector.normalized * velocity);
			}
			else {
				//Set the throw vector relative to the player
				Vector3 throwVector = new Vector3(deltaX,
												  xzMag,
												  deltaZ);
				this.sheepList[0].GetComponent<SheepMovement>().Throw (throwVector.normalized * this.throwForce);
			}
			this.sheepList.RemoveAt(0);
			if(this.sheepList.Count > 0){
				this.sheepList[0].GetComponent<SheepMovement>().Follow (this.gameObject);
			}
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

