//Author: 		Conor Hughes
//Date:			06/11/2015
//Description: 	This script is used for toggling on gameobjects over time in the home base, 
//				so some features will become unlocked over time.

using UnityEngine;
using System.Collections;

public class HomeBaseObjectManager : MonoBehaviour {

	private int currentLevelIndex;		//number of last level played

	public GameObject[] level1Objects, 	//arrays used to store groups of gameobjects which are toggled based on current index
						level2Objects,
						level3Objects,
						level4Objects,
						level5Objects,
						level6Objects,
						level7Objects,
						level8Objects,
						level9Objects;


	public void SetCurrentLevelIndex(int i){	//Sets current index and enables current objects
		currentLevelIndex = i;
		EnableCurrentLevelObjects();
	}

	// Use this for initialization
	void Awake () {
		DisableAllLevelObjects();
	}

	void DisableAllLevelObjects(){				//disables all objects
		foreach(GameObject g in level1Objects)g.SetActive(false);
		foreach(GameObject g in level2Objects)g.SetActive(false);
		foreach(GameObject g in level3Objects)g.SetActive(false);
		foreach(GameObject g in level4Objects)g.SetActive(false);
		foreach(GameObject g in level5Objects)g.SetActive(false);
		foreach(GameObject g in level6Objects)g.SetActive(false);
		foreach(GameObject g in level7Objects)g.SetActive(false);
		foreach(GameObject g in level8Objects)g.SetActive(false);
		foreach(GameObject g in level9Objects)g.SetActive(false);
	}

	void EnableCurrentLevelObjects(){			//enables objects based on current index
		if(currentLevelIndex > 0)foreach(GameObject g in level1Objects)g.SetActive(true);
		if(currentLevelIndex > 1)foreach(GameObject g in level2Objects)g.SetActive(true);
		if(currentLevelIndex > 2)foreach(GameObject g in level3Objects)g.SetActive(true);
		if(currentLevelIndex > 3)foreach(GameObject g in level4Objects)g.SetActive(true);
		if(currentLevelIndex > 4)foreach(GameObject g in level5Objects)g.SetActive(true);
		if(currentLevelIndex > 5)foreach(GameObject g in level6Objects)g.SetActive(true);
		if(currentLevelIndex > 6)foreach(GameObject g in level7Objects)g.SetActive(true);
		if(currentLevelIndex > 7)foreach(GameObject g in level8Objects)g.SetActive(true);
		if(currentLevelIndex > 8)foreach(GameObject g in level9Objects)g.SetActive(true);

	}

}
