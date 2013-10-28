using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LevelState
{
	INACTIVE,
	MOVING,
	ACTIVE
};

public enum SpawnType{ CHARACTER, SHEEP, WOLF, PIG };

public class NewLevelScript : MonoBehaviour
{
	public GameObject[] spawners;
	public GameObject 	nextLevel;
	public int 			levelTriggers = 1;
	public LevelState 	state = LevelState.INACTIVE;
	
	private Vector3 startPosition;
	private int triggered = 0;
	
	void Start(){
		this.startPosition = this.transform.position;
		this.transform.FindChild ("Ground").FindChild ("OutOfBounds").gameObject.collider.enabled = false;
		
		if (this.state == LevelState.INACTIVE)
			this.transform.position += new Vector3 (0.0f, 15.0f, 0.0f);
	}
	
	void Update(){
		switch (state) {
		case LevelState.INACTIVE:
			break;
		case LevelState.MOVING:
			if ((this.transform.position - startPosition).magnitude > 0.1f) {
				this.transform.position = Vector3.Lerp (this.transform.position, startPosition, Time.deltaTime * 0.5f);
			} else {
				state = LevelState.ACTIVE;
				this.transform.FindChild ("Ground").FindChild ("OutOfBounds").gameObject.collider.enabled = true;
				GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheepActions>().ResetSheepTargets();
			}
			break;
		case LevelState.ACTIVE:
			this.CheckSpawners();
			if (nextLevel != null){
				if( nextLevel.GetComponent<NewLevelScript>().state == LevelState.ACTIVE){
					state = LevelState.INACTIVE;
				}
			}
			break;
		}
	}
	
	public void CheckSpawners(){
		foreach (GameObject gobj in this.spawners){
			Spawnpoint script = gobj.GetComponent<Spawnpoint>();
			if (script.type == SpawnType.CHARACTER){
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				script.target = (player == null) ? this.SpawnObject(SpawnType.CHARACTER, gobj.transform.position) : player;
			}
			else{
				if (script.target == null){
					GameObject newObject = this.SpawnObject(script.type, gobj.transform.position);
					if (newObject != null) 
						script.target = newObject;
					else
						Debug.Break();
				}
			}
		}
	}
	
	private GameObject SpawnObject( SpawnType type, Vector3 location ){
		switch (type){
		case SpawnType.CHARACTER:
			GameObject character = Resources.Load("Character/Character", typeof(GameObject)) as GameObject;
			return GameObject.Instantiate(character, location, Quaternion.identity) as GameObject;
			break;
		case SpawnType.SHEEP:
			GameObject sheep = Resources.Load("Sheep/LittleSheep", typeof(GameObject)) as GameObject;
			return GameObject.Instantiate(sheep, location, Quaternion.identity) as GameObject;
			break;
		case SpawnType.WOLF:
			GameObject wolf = Resources.Load("Wolf/Wolf", typeof(GameObject)) as GameObject;
			return GameObject.Instantiate(wolf, location, Quaternion.identity) as GameObject;
			break;
		case SpawnType.PIG:
			GameObject pig = Resources.Load("Sheep/LittlePig", typeof(GameObject)) as GameObject;
			return GameObject.Instantiate(pig, location, Quaternion.identity) as GameObject;
			break;
		default:
			break;
		}
		return null;
	}
	/*
	public void SpawnCharacter ()
	{
		if (this.characterActive == null) {
			this.characterActive = Instantiate (this.character, this.characterSpawn.transform.position, this.characterSpawn.transform.rotation) as GameObject;
		}
	}

	public void SpawnSheep ()
	{
		for (int i = 0; i < sheepActive.Length; i++) {
			if (this.sheepActive [i] == null) {
				this.sheepActive [i] = Instantiate (this.sheep [i], this.sheepSpawn [i].transform.position, this.sheepSpawn [i].transform.rotation) as GameObject;
			}
		}
	}
	
	public void SpawnWolf ()
	{
		for (int i = 0; i < wolfActive.Length; i++) {
			if (this.wolfActive [i] == null) {
				this.wolfActive [i] = Instantiate (this.wolf[i], this.wolfSpawn [i].transform.position, this.wolf[i].transform.rotation) as GameObject;
			}
		}
	}
	*/
	public void NextLevel ()
	{
		triggered ++;
		if (triggered >= levelTriggers){
			
			nextLevel.GetComponent<NewLevelScript>().state = LevelState.MOVING;
			//nextLevel.GetComponent<LevelScript>().characterActive = this.characterActive;
			foreach (GameObject s in GameObject.FindGameObjectsWithTag("Sheep")) {
				if (s != null){
					GameObject newDeathParts = GameObject.Instantiate(s.GetComponent<SheepMovement>().deathParts, s.transform.position, Quaternion.identity) as GameObject;
					GameObject.Destroy(newDeathParts, 1.0f);
					Destroy (s);
				}
			}
			GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheepActions>().ResetSheepTargets();
		}
	}
	/*
	public GameObject character;
	public GameObject characterSpawn;
	public GameObject[] sheep;
	public GameObject[] sheepSpawn;
	public GameObject[] wolf;
	public GameObject[] wolfSpawn;
	public GameObject nextLevel;
	public int levelTriggers = 1;
	public LevelState state = LevelState.INACTIVE;
	
	private GameObject characterActive;
	private GameObject[] wolfActive;
	private GameObject[] sheepActive;
	
	private Vector3 startPosition;
	private int triggered = 0;

	// Use this for initialization
	void Start ()
	{
		sheepActive = new GameObject[sheep.Length];
		wolfActive = new GameObject[wolf.Length];
		startPosition = transform.position;
		if (state == LevelState.ACTIVE) {
			SpawnCharacter ();
			SpawnSheep ();
			SpawnWolf ();
		} else {
			this.transform.position += new Vector3 (0.0f, 15.0f, 0.0f);
			this.transform.FindChild ("Ground").FindChild ("OutOfBounds").gameObject.collider.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (state) {
		case LevelState.INACTIVE:
			break;
		case LevelState.MOVING:
			//SpawnCharacter ();
			SpawnSheep ();
			//SpawnWolf ();
			if ((this.transform.position - startPosition).magnitude > 0.1f) {
				this.transform.position = Vector3.Lerp (this.transform.position, startPosition, Time.deltaTime * 0.5f);
			} else {
				state = LevelState.ACTIVE;
				this.transform.FindChild ("Ground").FindChild ("OutOfBounds").gameObject.collider.enabled = true;
				GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheepActions>().ResetSheepTargets();
			}
			break;
		case LevelState.ACTIVE:
			SpawnCharacter ();
			SpawnSheep ();
			SpawnWolf ();
			if( nextLevel.GetComponent<LevelScript>().state == LevelState.ACTIVE){
				state = LevelState.INACTIVE;
			}
			break;
		}
	}

	public void SpawnCharacter ()
	{
		if (this.characterActive == null) {
			this.characterActive = Instantiate (this.character, this.characterSpawn.transform.position, this.characterSpawn.transform.rotation) as GameObject;
		}
	}

	public void SpawnSheep ()
	{
		for (int i = 0; i < sheepActive.Length; i++) {
			if (this.sheepActive [i] == null) {
				this.sheepActive [i] = Instantiate (this.sheep [i], this.sheepSpawn [i].transform.position, this.sheepSpawn [i].transform.rotation) as GameObject;
			}
		}
	}
	
	public void SpawnWolf ()
	{
		for (int i = 0; i < wolfActive.Length; i++) {
			if (this.wolfActive [i] == null) {
				this.wolfActive [i] = Instantiate (this.wolf[i], this.wolfSpawn [i].transform.position, this.wolf[i].transform.rotation) as GameObject;
			}
		}
	}
	
	public void NextLevel ()
	{
		triggered ++;
		if (triggered >= levelTriggers){
			nextLevel.GetComponent<LevelScript> ().state = LevelState.MOVING;
			nextLevel.GetComponent<LevelScript> ().characterActive = this.characterActive;
			foreach (GameObject s in sheepActive) {
				if (s != null){
					GameObject newDeathParts = GameObject.Instantiate(s.GetComponent<SheepMovement>().deathParts, s.transform.position, Quaternion.identity) as GameObject;
					GameObject.Destroy(newDeathParts, 1.0f);
					Destroy (s);
				}
			}
			GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheepActions>().ResetSheepTargets();
		}
	}
	*/
}