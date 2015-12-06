//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	The behaviour of the path indicator. Draws out a path of where the player will move.

using UnityEngine;
using System.Collections;

public class PathIndicator : MonoBehaviour {
	
	NavMeshPath 		path;						//current nav mesh path
	NavMeshAgent		playerAgent;				//agent component
	
	bool 				pathSet = false;			//if true, path is set

	int 				currentPathIndex = 1;		//current corner index of path

	float 				distanceToNextCorner = 100;	//distance to the next corner on path

	GameObject	 		destinationObject, 			//the path destination object
						arrowObject, 				//the arrow indicator object
						playerObject;				//the player object

	TimeScaler 			timeScale;					//time scale instance

	float 				pathDistance = 1.0f, 		//distance to next point on path
						distanceFromLastArrow = 100;//distance from the last arrow

	public float 		lastInterval, 				//time of last frame
						timeNow, 					//current time
						customDeltaTime;			//custom delta time var

	//Runs once on start
	void Start(){
		timeScale = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerAgent = playerObject.GetComponent<NavMeshAgent>();
	}

	//Instantiates a new arrow object
	void CreatePathArrow(){
		arrowObject = Instantiate(Resources.Load("Path Arrow"), transform.position + new Vector3(0,0.1f,0), Quaternion.identity) as GameObject;
		arrowObject.transform.eulerAngles = transform.eulerAngles;
	}
	
	// Use this for initialization
	public void SetPath (NavMeshPath newPath) {
		path = newPath;
		pathSet = true;
		pathDistance = Vector3.Distance(transform.position, path.corners[path.corners.Length - 1]);
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
				SmoothLookAt(path.corners[currentPathIndex]);
			}
			else Destroy(gameObject);
		}
		lastInterval = timeNow;
	}

	//Look at target with damping
	void SmoothLookAt(Vector3 targetPosition){
		Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, customDeltaTime * 8.5f);
	}
}
