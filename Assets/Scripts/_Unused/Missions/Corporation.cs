using UnityEngine;
using System.Collections;

public class Corporation : MonoBehaviour {

	public string  corporationName = "Test Name", 
			corporationDescription = "This a a basic description",
			difficulty = "Very Easy";

	public Texture corporationLogo;

	public Color[] corporationColors;

	public Corporation(string c_name, string c_desc, string c_diff, Texture c_logo, Color[] c_colors){
		corporationName = c_name;
		corporationDescription = c_desc;
		difficulty = c_diff;
		corporationLogo = c_logo;
		corporationColors = c_colors;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IncreaseDifficultyForCorporation(){
		switch(difficulty){
			case "Very Easy" : 	difficulty = "Easy"; break;
			case "Easy" : 		difficulty = "Normal"; break;
			case "Normal" : 	difficulty = "Hard"; break;
			case "Hard" : 		difficulty = "Very Hard"; break;
			case "Very Hard" : 	difficulty = "Very Hard"; break;
		default:				difficulty = "Normal"; break;
		}
	}

}
