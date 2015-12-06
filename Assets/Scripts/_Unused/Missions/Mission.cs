using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour {

	public string  
			missionName,			//Title of mission.
			missionDescription,		//Overview of what to do in mission.
			missionObjective,		//Target item for the mission.
			missionTheme,			//Hidden string used to determine furnishings in level.
			difficulty;				//Level of challenge for mission, ranges from very easy to very hard.

	public int daysRemaining, 		//
			   objectiveValue;		//

	public Corporation targetCorporation;
	CorporationManager corpManager;

	public Mission(string m_name, string m_desc, string m_obj, int m_objVal, string m_theme, string m_diff, int m_daysLeft){
		this.missionName 		= m_name;
		this.missionDescription 	= m_desc;
		this.missionObjective 	= m_obj;
		this.missionTheme 		= m_theme;
		this.difficulty 			= m_diff;
		this.daysRemaining 		= m_daysLeft;
		this.objectiveValue		= m_objVal;
	}

	// Use this for initialization
	void Start () {
		print(difficulty + " : " +objectiveValue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SubtractRemainingDays(int subtractedAmount){
		daysRemaining = daysRemaining - subtractedAmount;
	}
}
