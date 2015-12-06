//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script manages the in-game timescale, which affects everything but the player.

using UnityEngine;
using System.Collections;

public class PlayerPathIndicator : MonoBehaviour {

	public 	Vector3 		targetPosition = Vector3.zero;	//target destination of the indicator
	private NavMeshAgent 	agent;							//agent component of indicator
	private	TimeScaler 		tt;								//instance of the timescaler

	LineRenderer line;										//line renderer component used to debug path

	private bool 			runOnce = false, 				//indicates path once
							pathDrawn = false,				//indicates if path is on
	 						pingInProgress = false;			//shows if indicator is active


	private float 			lineWidth = 0f, 				//width of line renderer
							targetWidth = 0.05f;			//width of line start

	private GameObject 		pingObject, 					//the ping gameobject
							playerObject;					//the player object

	PathPing 				pPing;							//instance of path ping script

	//Sets the player gameobject
	public void SetPlayer(GameObject g){
		playerObject = g;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetPosition != Vector3.zero){
			if(runOnce == false){
				line = GetComponent<LineRenderer>();
				line.SetWidth(lineWidth, lineWidth);
				agent = GetComponent<NavMeshAgent>();
				GetPath ();
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

	//Self destruct the gameobject
	void DestroySelf(){
		Destroy(gameObject);
	}

	//Gets the current path
	void GetPath(){
		line.SetPosition(0, transform.position);
		agent.SetDestination(targetPosition);

		DrawPath(agent.path);
		agent.Stop();

	}

	//Draws the current path
	void DrawPath(NavMeshPath path){
		if(path.corners.Length < 2) return;
		line.SetVertexCount(path.corners.Length);
		for(int i = 0; i < path.corners.Length; i++){
			line.SetPosition(i, path.corners[i]);
		}
		pathDrawn = true;
	}
}
