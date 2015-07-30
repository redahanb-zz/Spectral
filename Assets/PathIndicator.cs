using UnityEngine;
using System.Collections;

public class PathIndicator : MonoBehaviour {
	
	CharacterController controller;
	
	NavMeshPath 		path;
	
	bool 				pathSet = false;

	int 				currentPathIndex = 1;

	float 				distanceToNextCorner = 100;

	GameObject	 		destinationObject;

	TimeScaler timeScale;

	void Start(){
		timeScale = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
	}
	
	// Use this for initialization
	public void SetPath (NavMeshPath newPath) {
		path = newPath;
		pathSet = true;
	}
	
	// Update is called once per frame
	void Update () {

		if(path.corners.Length < 2) Destroy(gameObject);

		if(pathSet){
			if(distanceToNextCorner < 0.6f){
				distanceToNextCorner = 100;	
				currentPathIndex = currentPathIndex + 1;
			}

			if(currentPathIndex != path.corners.Length){ 
					distanceToNextCorner = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
					transform.position = Vector3.MoveTowards(transform.position, path.corners[currentPathIndex], 0.1f);
					transform.LookAt(path.corners[currentPathIndex]);
			}

		}
	}
}
