using UnityEngine;
using System.Collections;

public abstract class TriggeredEvent : MonoBehaviour
{
	protected bool triggered = false;
	
	public void Activate(){ this.triggered = true; }
	
	void Update(){
		if (this.triggered){
			this.Execute();
		}
	}
	
	protected abstract void Execute();
}

