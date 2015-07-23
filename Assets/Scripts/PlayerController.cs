using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	bool timeStopped = false;

	float myDeltaTime;

	float speed = 0.4f;

	// Use this for initialization
	void Start () {
		myDeltaTime = Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		//print(timeStopped + " : " + Time.timeScale);
		if(Input.GetKeyDown(KeyCode.T)){
			timeStopped = !timeStopped;
			print(timeStopped +" : T was pressed!!!");
		}

		if(timeStopped)
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0, myDeltaTime * speed);
		else
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, myDeltaTime * speed);
	}
}
