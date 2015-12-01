using UnityEngine;
using System.Collections;

public class LevelDoorTrigger : MonoBehaviour {

	LevelManager lvlManager;
	
	public int levelIndex;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!lvlManager){
			lvlManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		}
	}
	
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			lvlManager.SetCurrentDoor(transform);
			lvlManager.levelIndex = levelIndex;
		}
	}
	
	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			lvlManager.levelIndex = 0;
		}
	}
}
