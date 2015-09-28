using UnityEngine;
using System.Collections;

public class ObjectHover : MonoBehaviour {
	bool hoverUp = true;
	float hoverDistance = 100;
	Vector3 upPos, downPos, nextPos;

	float hoverSpeed = 0.5f;

	bool canMove = false, runOnce = false;

	float startDelay = 1;

	// Use this for initialization
	void Start () {
		upPos 	= transform.position + new Vector3(0, 0.5f,0);	
		downPos = transform.position + new Vector3(0,-0.5f,0);

		int randomNum = Random.Range(0,2);
		if(randomNum == 1)hoverUp = true;
		else hoverUp = false;

		startDelay = Random.Range(0.1f, 1.5f);
		Invoke("ToggleCanMove", startDelay);
	}

	void ToggleCanMove(){
		canMove = !canMove;
		runOnce = false;
	}
	
	// Update is called once per frame
	void Update () {
		hoverDistance = Vector3.Distance(transform.position, nextPos);
		if(hoverDistance < 0.05f){
				if(!runOnce){
				runOnce = true;
				hoverUp = !hoverUp;
				ToggleCanMove();
				Invoke("ToggleCanMove",1);
			}
		}

		if(hoverUp)nextPos = upPos;
		else nextPos = downPos;

		if(canMove)transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * hoverSpeed);
	}
}
