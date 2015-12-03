using UnityEngine;
using System.Collections;

public class PlayerPathIndicator : MonoBehaviour {

	public Vector3 targetPosition = Vector3.zero;
	NavMeshAgent agent;
	TimeScaler tt;

	LineRenderer line;

	bool runOnce = false, pathDrawn = false;

	float lineWidth = 0f, targetWidth = 0.05f;


	bool pingInProgress = false;

	GameObject pingObject, playerObject;
	PathPing pPing;


	// Use this for initialization
	void Start () {
		//Invoke("DestroySelf", 5);

		//tt = GameObject.Find("TimeManager").GetComponent<TimeScaler>();
	}

	public void SetPlayer(GameObject g){
		playerObject = g;
	}
	
	// Update is called once per frame
	void Update () {
		//print(pathDrawn + " : " +lineWidth);


		if(targetPosition != Vector3.zero){
			if(runOnce == false){
				line = GetComponent<LineRenderer>();
				line.SetWidth(lineWidth, lineWidth);
				agent = GetComponent<NavMeshAgent>();
				GetPath ();
				//runOnce = true;


			}
		}



		if(pathDrawn){
			lineWidth = Mathf.Lerp(lineWidth, targetWidth, 0.05f);
			line.SetWidth(lineWidth,lineWidth);

			if(!pingObject){
				pingObject = Instantiate(Resources.Load("PathPing"), playerObject.transform.position + (playerObject.transform.forward * 1), Quaternion.identity) as GameObject;
				pingObject.transform.eulerAngles = playerObject.transform.eulerAngles;
				pPing = pingObject.GetComponent<PathPing>();
				pPing.SetPath(agent.path, playerObject);
			}
		}
	}

	void DestroySelf(){
		Destroy(gameObject);
	}

	void GetPath(){
		line.SetPosition(0, transform.position);
		agent.SetDestination(targetPosition);

		DrawPath(agent.path);
		agent.Stop();

	}

	void DrawPath(NavMeshPath path){
		if(path.corners.Length < 2) return;
		line.SetVertexCount(path.corners.Length);
		for(int i = 0; i < path.corners.Length; i++){
			line.SetPosition(i, path.corners[i]);
		}
		pathDrawn = true;
	}
}
