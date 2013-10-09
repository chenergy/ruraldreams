using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSheepActions : MonoBehaviour
{
	public float		throwForce = 5.0f;
	public bool			throwToPoint = false;
	public GameObject 	littleSheep;
	public GameObject 	bigSheep;
	
	[HideInInspector]
	public List<GameObject> sheepList;
	
	// Use this for initialization
	void Start ()
	{
		sheepList = new List<GameObject>();
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
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button14)){
			this.CycleSheepBack();
		}
		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button13)){
			this.CycleSheepFront();
		}
	}
	
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
				float velocity = Mathf.Sqrt ((Physics.gravity.y * Mathf.Pow (xzMag, 2))/(xzMag*Mathf.Sin (2*angle)-deltaY*Mathf.Cos (2*angle)-deltaY));
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
			this.sheepList[0].GetComponent<SheepMovement>().Follow (this.sheepList[this.sheepList.Count -1]);
			this.sheepList[1].GetComponent<SheepMovement>().Follow (this.gameObject);
			this.sheepList.Add(this.sheepList[0]);
			this.sheepList.RemoveAt(0);
		}
	}
	
	public void CycleSheepFront(){
		if (this.sheepList.Count > 1){
			this.sheepList[this.sheepList.Count-1].GetComponent<SheepMovement>().Follow (this.gameObject);
			this.sheepList[0].GetComponent<SheepMovement>().Follow (this.sheepList[this.sheepList.Count-1]);
			this.sheepList.Insert(0, this.sheepList[this.sheepList.Count-1]);		
			this.sheepList.RemoveAt(this.sheepList.Count-1);
		}
	}
}

