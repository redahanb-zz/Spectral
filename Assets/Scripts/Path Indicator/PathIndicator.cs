﻿using UnityEngine;
using System.Collections;

public class PathIndicator : MonoBehaviour {
	
	CharacterController controller;
	
	NavMeshPath 		path;
	NavMeshAgent		playerAgent;
	
	bool 				pathSet = false;

	int 				currentPathIndex = 1;

	float 				distanceToNextCorner = 100;

	GameObject	 		destinationObject, arrowObject, playerObject;

	TimeScaler timeScale;

	float pathDistance = 1.0f, distanceFromLastArrow = 100;

	public float lastInterval, timeNow, customDeltaTime;


	void Start(){
		timeScale = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerAgent = playerObject.GetComponent<NavMeshAgent>();
	}

	void CreatePathArrow(){
		//if(timeScale.currentScale < 0.11f){

		arrowObject = Instantiate(Resources.Load("Path Arrow"), transform.position + new Vector3(0,0.1f,0), Quaternion.identity) as GameObject;
		arrowObject.transform.eulerAngles = transform.eulerAngles;
		//}
	}
	
	// Use this for initialization
	public void SetPath (NavMeshPath newPath) {
		path = newPath;
		pathSet = true;
		pathDistance = Vector3.Distance(transform.position, path.corners[path.corners.Length - 1]);
		//print(pathDistance);
		//InvokeRepeating("CreatePathArrow", 0.01f, pathDistance/200);

	}
	
	// Update is called once per frame
	void Update () {
		timeNow = Time.realtimeSinceStartup;
		customDeltaTime = timeNow - lastInterval;


		if(arrowObject)distanceFromLastArrow = Vector3.Distance(transform.position, arrowObject.transform.position);
		if(distanceFromLastArrow > 0.25f)CreatePathArrow();

		if(path.corners.Length < 2) Destroy(gameObject);

		if(pathSet){
			if(distanceToNextCorner < 0.6f){
				distanceToNextCorner = 100;	
				currentPathIndex = currentPathIndex + 1;
			}

			if(currentPathIndex != path.corners.Length){ 
				distanceToNextCorner = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
				transform.position = Vector3.MoveTowards(transform.position, path.corners[currentPathIndex], customDeltaTime * 5);
				//transform.LookAt(path.corners[currentPathIndex]);
				SmoothLookAt(path.corners[currentPathIndex]);
			}
			else{ 
				//print("END OF PATH REACHED");
				Destroy(gameObject);
			}

		}
		lastInterval = timeNow;
	}

	void SmoothLookAt(Vector3 targetPosition){
		Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, customDeltaTime * 8.5f);
	}
}