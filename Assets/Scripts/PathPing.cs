using UnityEngine;
using System.Collections;

public class PathPing : MonoBehaviour {

	CharacterController controller;

	NavMeshPath path;

	bool pathSet = false;
	int currentPathIndex = 1;
	float distanceToNextCorner = 100;

	GameObject playerObject;

	void Start(){
		controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	public void SetPath (NavMeshPath newPath, GameObject g) {
		path = newPath;
		playerObject = g;
		//transform.forward = g.transform.forward;
		//transform.eulerAngles = g.transform.eulerAngles;
		pathSet = true;
	}
	
	// Update is called once per frame
	void Update () {



		if(pathSet){



			if(distanceToNextCorner < 0.6f){
				distanceToNextCorner = 100;	
				currentPathIndex = currentPathIndex + 1;
			}
			if(currentPathIndex != path.corners.Length){ 
				distanceToNextCorner = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
				print("Length: " +path.corners.Length +"   Index: " +currentPathIndex);


				transform.Translate((transform.position - path.corners[currentPathIndex]) * 0.05f);
				transform.LookAt(path.corners[currentPathIndex]);
			}
			else{
				print("End of path reached");
			}
		}
	}
}
