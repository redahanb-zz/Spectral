using UnityEngine;
using System.Collections;

public class CorporationManager : MonoBehaviour {

	Corporation testCorp;


	// Use this for initialization
	void Start () {
	
	}

	void CreateCorporations(){

		Color[] testCorpColors = new Color[5]{Color.red, Color.magenta, Color.green, Color.yellow, Color.white};
		Texture textCorpLogo = Resources.Load("Iorex Logo") as Texture;
		string testCorpDesc = "This is the description for the test corporation";

		testCorp = new Corporation("Test Corp", testCorpDesc, "Very Easy", textCorpLogo, testCorpColors);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
