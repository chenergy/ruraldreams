using UnityEngine;
using System.Collections;

public class SheepPickupRadius : MonoBehaviour
{
	void Update(){
		this.gameObject.transform.localPosition = Vector3.zero;
		this.gameObject.transform.localRotation = Quaternion.identity;
	}
	
	void OnTriggerStay(Collider other){
		if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button17)){
			if (other.tag == "Player"){
				if (this.transform.parent.GetComponent<SheepMovement>().state == SheepState.IDLE){
					other.GetComponent<CharacterSheepActions>().PickupSheep( this.transform.parent.gameObject );
				}
			}
		}
	}
}

