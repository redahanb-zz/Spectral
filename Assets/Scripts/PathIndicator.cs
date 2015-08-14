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
		InvokeRepeating("CreatePathArrow", 0.01f, 0.008f);
	}

	void CreatePathArrow(){
		if(timeScale.currentScale < 0.11f){

		GameObject arrowObject = Instantiate(Resources.Load("Path Arrow"), transform.position + new Vector3(0,0.1f,0), Quaternion.identity) as GameObject;
		arrowObject.transform.eulerAngles = transform.eulerAngles;
		}
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
					transform.position = Vector3.MoveTowards(transform.position, path.corners[currentPathIndex], 0.08f);
					transform.LookAt(path.corners[currentPathIndex]);
			}
			else{ 
				//print("END OF PATH REACHED");
				Destroy(gameObject);
			}

		}
	}
}
