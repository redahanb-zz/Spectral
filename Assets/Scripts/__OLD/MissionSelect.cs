using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MissionSelect : MonoBehaviour {

	struct Mission{
		private string missionType, missionDescription, missionTarget, missionTheme, difficulty;
		private int daysLeft;
		private Texture missionTargetLogo;

		public Mission(string type, string des, string target, string theme, string diff, int days, Texture logo){
			missionType = type;
			missionDescription = des;
			missionTarget = target;
			missionTheme = theme;
			difficulty = diff;
			daysLeft = days;
			missionTargetLogo = logo;
		}

		public string mission_type{
			get {return missionType;}
			set {missionType = value;}
		}

		public string mission_description{
			get {return missionDescription;}
			set {missionDescription = value;}
		}

		public string mission_target{
			get {return missionTarget;}
			set {missionTarget = value;}
		}

		public string mission_theme{
			get {return missionTheme;}
			set {missionTheme = value;}
		}

		public Texture mission_logo{
			get {return missionTargetLogo;}
			set {missionTargetLogo = value;}
		}

		public string mission_difficulty{
			get {return difficulty;}
			set {difficulty = value;}
		}

		public int days_left{
			get {return daysLeft;}
			set {daysLeft = value;}
		}
	}

	List<Mission> missionList = new List<Mission>();

	RectTransform rTransform;
	Text txt;

	GameObject missionListObject;
	// Use this for initialization
	void Start () {
		//missionList.Add(new Mission(());
		for(int i = 0; i < 20; i++){
			GenerateMission();
		}

//		foreach (Mission m in missionList) // Loop through List with foreach.
//		{
//			print("FOREACH: " +m.mission_difficulty);
//		}

		missionListObject = GameObject.Find("Canvas").transform.Find("ScrollableWindow").Find("MissionList").gameObject;
		missionListObject.GetComponent<RectTransform>().sizeDelta = new Vector2(900, missionList.Count * 80 );

		for(int i =0; i < missionList.Count; i++){
			//print("FOR: "  +missionList[i].mission_difficulty);
			GameObject missionObject = Instantiate(Resources.Load("Mission"), transform.position, Quaternion.identity) as GameObject;
			missionObject.transform.parent = missionListObject.transform;
			missionObject.GetComponent<RectTransform>().localPosition = new Vector3(0,(-i*80) + (((missionList.Count/2) * 80) - 40),0);
			missionObject.transform.Find("Days").GetComponent<Text>().text = missionList[i].days_left.ToString();
			missionObject.transform.Find("Difficulty").GetComponent<Text>().text = missionList[i].mission_difficulty;
			missionObject.transform.Find("Title").GetComponent<Text>().text = missionList[i].mission_description;

			if(i == 0)missionObject.GetComponent<MissionListing>().isSelected = true;
		}

		GameObject scrollbarObject = Instantiate(Resources.Load("Scrollbar")) as GameObject;
		scrollbarObject.transform.parent = missionListObject.transform.parent.parent;
		scrollbarObject.GetComponent<RectTransform>().localPosition = new Vector3(470,0,0);
		scrollbarObject.GetComponent<Scrollbar>().value = 1;


		missionListObject.transform.parent.GetComponent<ScrollRect>().verticalScrollbar = scrollbarObject.GetComponent<Scrollbar>();


	}



	void GenerateMission(){
		string m_type = "Heist",
		 	   m_description = "Description goes here",
		 	   m_target = "Target",
		 	   m_difficulty = "Easy",
		 	   m_theme = "Office";
		int m_days = 5;
		Texture m_logo = new Texture();

		missionList.Add(new Mission(m_type, m_description, m_target, m_theme, m_difficulty, m_days, m_logo));
	}

	string CalculateDifficulty(){
		string difficulty = "Easy";

		return difficulty;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
