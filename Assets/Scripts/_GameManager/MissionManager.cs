using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour {

	List<Mission> missionList 		= new List<Mission>();
	List<Mission> sortedMissionList = new List<Mission>();

	List<GameObject> missionGameObjectList = new List<GameObject>();

	float missionListingHeight = 45;
	float missionListXPos = 66;

	GameObject missionListObject, missionDetailsObject;

	int selecttedMissionIndex = 4, lastMissionIndex;

	struct Objective{
		public string objectiveName;
		public int objectiveValue;
		public Objective(string name, int value){
			objectiveName = name;
			objectiveValue = value;
		}
	}

	// Use this for initialization
	void Start () {
		missionDetailsObject = GameObject.Find("Mission Details");
		for(int i = 0; i < 20; i++){
			GenerateMission();
		}
		SortList();
		DisplayMissionsList();
		selecttedMissionIndex = sortedMissionList.Count-1;
		DisplayMissionDetails();
	}
	
	// Update is called once per frame
	void Update () {
		missionListObject.GetComponent<RectTransform>().localPosition = Vector3.Lerp(missionListObject.GetComponent<RectTransform>().localPosition,
		                                                                             new Vector3((-Screen.width/2 + (selecttedMissionIndex * -7))+320,0,0),
		                                                                             Time.deltaTime*5);

		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
			if(selecttedMissionIndex + 1 < sortedMissionList.Count){
				lastMissionIndex = selecttedMissionIndex;
				selecttedMissionIndex = selecttedMissionIndex + 1;
			}
			else{
				lastMissionIndex = selecttedMissionIndex;
				selecttedMissionIndex = 0;
			}
			DisplayMissionDetails();
		}
		else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
			if(selecttedMissionIndex - 1 > -1){
				lastMissionIndex = selecttedMissionIndex;
				selecttedMissionIndex = selecttedMissionIndex - 1;
			}
			else{
				lastMissionIndex = selecttedMissionIndex;
				selecttedMissionIndex = sortedMissionList.Count - 1;
			}
			DisplayMissionDetails();
		}
		missionGameObjectList[selecttedMissionIndex].GetComponent<MissionListing>().isSelected = true;
		missionGameObjectList[lastMissionIndex].GetComponent<MissionListing>().isSelected = false;
	}
	
	void GenerateMission(){
		string 	m_theme = "Office",
				m_difficulty = GenerateDifficulty();

		Objective m_objective = GenerateMissionObjective(m_theme, m_difficulty);

		string 
		m_name = GenerateMissionName(m_objective.objectiveName, m_theme),
		m_description = "Description goes here";
		
		int m_days = GenerateNumDays(m_difficulty);
		Texture m_logo = new Texture();

		Mission m = new Mission(m_name, m_description, m_objective.objectiveName, m_objective.objectiveValue, m_theme, m_difficulty, m_days);
		missionList.Add(m);
	}

	void SortList(){
		foreach(Mission m in missionList){if(m.difficulty == "Very Hard")AddMission(m);}
		foreach(Mission m in missionList){if(m.difficulty == 	  "Hard")AddMission(m);}
		foreach(Mission m in missionList){if(m.difficulty == 	"Normal")AddMission(m);}
		foreach(Mission m in missionList){if(m.difficulty == 	  "Easy")AddMission(m);}
		foreach(Mission m in missionList){if(m.difficulty == "Very Easy")AddMission(m);}
	}

	void AddMission(Mission newMission){
		print (newMission.difficulty +" : " + newMission.objectiveValue);
		Mission sortedMission = new Mission(newMission.missionName, newMission.missionDescription, newMission.missionObjective, newMission.objectiveValue, newMission.missionTheme, newMission.difficulty, newMission.daysRemaining); 
		sortedMissionList.Add(sortedMission);
	}

	Objective GenerateMissionObjective(string missionTheme, string missionDifficulty){
		Objective missionObjective = new Objective("File", 9999);

		if(missionTheme == "Office"){
			int objectiveValue = 1000;
			switch(missionDifficulty){
				case "Very Easy" : objectiveValue = (Random.Range(3,5)   * 1000); break;
				case "Easy" 	 : objectiveValue = (Random.Range(8,10)  * 1000); break;
				case "Normal" 	 : objectiveValue = (Random.Range(13,15) * 1000); break;
				case "Hard" 	 : objectiveValue = (Random.Range(18,20) * 1000); break;
				case "Very Hard" : objectiveValue = (Random.Range(23,25) * 1000); break;
				default : 		   objectiveValue = (Random.Range(3,5)   * 1000); break;
			}

			int randomNum = Random.Range(0,6);
			switch(randomNum){
				case 1  :missionObjective = new Objective("File", objectiveValue); break;
				case 2  :missionObjective = new Objective("Wristwatch", objectiveValue);  break;
				case 3  :missionObjective = new Objective("Book", objectiveValue);  break;
				case 4  :missionObjective = new Objective("Keycard", objectiveValue);  break;
				case 5  :missionObjective = new Objective("Data Storage Device", objectiveValue);  break;
				default :missionObjective = new Objective("File", objectiveValue); break;
			}
		}
		return missionObjective;
	}

	string GenerateMissionName(string missionObjective, string missionTheme){
		string missionName = "Basic mission description goes here", missionlocation = "Location;";
		int randomNum = Random.Range(0,5);
		if(missionTheme == "Office"){
			switch(randomNum){
				case 1 :missionlocation = "Office Building"; break;
				case 2 :missionlocation = "Offices"; break;
				case 3 :missionlocation = "Bureau"; break;
				case 4 :missionlocation = "Tower"; break;
				case 5 :missionlocation = "Office"; break;
				default :missionlocation = "Office"; break;
			}
		}
		randomNum = Random.Range(0,5);
		switch(randomNum){
			case 1 :missionName = "Retrieve the " +missionObjective +" from the " +missionlocation; break;
			case 2 :missionName = "Steal a " +missionObjective +" from  " +missionlocation; break;
			case 3 :missionName = "Obtain the " +missionObjective +" out of " +missionlocation; break;
			case 4 :missionName = "Acquire the " +missionObjective +" from " +missionlocation; break;
			case 5 :missionName = "Take the " +missionObjective +" out from the " +missionlocation; break;
			default :missionName = "Obtain the " +missionObjective +" from the " +missionlocation; break;
		}
		return missionName;
	}

	string GenerateDifficulty(){
		string difficulty = "Normal";
		int randomNum = Random.Range(0,6);
		switch(randomNum){
			case 1: difficulty = "Very Easy"; break;
			case 2: difficulty = "Easy"; break;
			case 3: difficulty = "Normal"; break;
			case 4: difficulty = "Hard"; break;
			case 5: difficulty = "Very Hard"; break;
			default: difficulty = "Normal"; break;
		}
		return difficulty;
	}

	int GenerateNumDays(string missionDifficulty){
		int days;
		if(missionDifficulty == "Very Easy"){
			days = Random.Range(15,20);
		}
		else if(missionDifficulty == "Easy"){
			days = Random.Range(8,14);
		}
		else if(missionDifficulty == "Normal"){
			days = Random.Range(7,12);
		}
		else if(missionDifficulty == "Hard"){
			days = Random.Range(4,7);
		}
		else if(missionDifficulty == "Very Hard"){
			days = Random.Range(2,5);
		}
		else{
			days = 10;
		}
		return days;
	}

	void DisplayMissionsList(){
		missionListObject = GameObject.Find("Canvas").transform.Find("MissionMenu").Find("MissionSelect").Find("MissionList").gameObject;
		missionListObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, (missionList.Count * missionListingHeight) + 5 );
		missionListObject.GetComponent<RectTransform>().localPosition = new Vector3((-Screen.width/2)+300,0,0);

		for(int i =0; i < sortedMissionList.Count; i++){
			GameObject missionObject = Instantiate(Resources.Load("UI/MissionListing"), transform.position, Quaternion.identity) as GameObject;
			missionObject.transform.parent = missionListObject.transform;
			missionObject.GetComponent<RectTransform>().localPosition = new Vector3(-66+(i*7),	(i*missionListingHeight) - ((sortedMissionList.Count/2)*missionListingHeight) + (0),	0);
			missionObject.transform.Find("DaysLeft").GetComponent<Text>().text = sortedMissionList[i].daysRemaining.ToString();
			missionObject.transform.Find("Difficulty").GetComponent<Text>().text = sortedMissionList[i].difficulty;
			missionObject.transform.Find("Title").GetComponent<Text>().text = sortedMissionList[i].missionName;
			missionObject.GetComponent<MissionListing>().SetDifficultyColor(sortedMissionList[i].difficulty);
			missionGameObjectList.Add(missionObject);
		}
	}

	void DisplayMissionDetails(){
		missionDetailsObject.transform.localPosition = new Vector3((Screen.width/2) - (missionDetailsObject.GetComponent<RectTransform>().sizeDelta.x/2),0,0);
		missionDetailsObject.transform.Find("Objective").Find("ObjectiveDescription Part1").GetComponent<Text>().text = sortedMissionList[selecttedMissionIndex].missionDescription;
		missionDetailsObject.transform.Find("Target Item").Find("TargetDescription").GetComponent<Text>().text = "Item: " +sortedMissionList[selecttedMissionIndex].missionObjective + "\n Value: " +sortedMissionList[selecttedMissionIndex].objectiveValue +" credits";	
	}
}
