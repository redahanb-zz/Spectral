using UnityEngine;
using System.Collections;

public class testLevelLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadStage(int index){
		Application.LoadLevel (index);
	}

	public void loadStage3(){
		Application.LoadLevel (1);
	}

	public void loadStage5(){
		Application.LoadLevel (2);
	}

	public void loadDataStream(){
		Application.LoadLevel (3);
	}
}
