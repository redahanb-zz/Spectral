using UnityEngine;
using System.Collections;

public class PlayerNavmeshTest : MonoBehaviour {

	TimeTest 		timetest;

	Animator 		playerAnimator;
	NavMeshAgent 	agent;

	Ray 			ray;
	RaycastHit 		rayHit;

	float 			distance = 0f,
					moveSpeed = 1f;

	Vector3 		targetPosition;

	GameObject 		pathObject, 
					destinationObject;

	bool 			canMove = false,
					hasPath = false;

	enum 			MoveState{ Idle, CrouchWalk, Run};
	MoveState 		currentMoveState = MoveState.Idle;

	// Use this for initialization
	void Start () {
		playerAnimator 	= GetComponent<Animator>();
		agent 			= GetComponent<NavMeshAgent>();
		timetest 		= GameObject.Find("Time Manager").GetComponent<TimeTest>();
	}
	
	// Update is called once per frame
	void Update () {
		hasPath = agent.hasPath;
		MoveStateManager();
		GetInput();
		DebugScript();
	}

	//Use for debugging
	void DebugScript(){
//		print("[PlayerNavmeshTest] Move State: " +(int)currentMoveState +"   " +
//			  "[Animator] Move State: " +playerAnimator.GetInteger("moveState") + "   " +
//			  "Distance: " +distance);
	}

	//Assigns action per state and sets the animator move state
	void MoveStateManager(){
		playerAnimator.SetInteger("moveState", (int)currentMoveState);
		switch(currentMoveState){
			case MoveState.Idle: 		Idle(); 		break;
			case MoveState.CrouchWalk: 	CrouchWalk(); 	break;
			case MoveState.Run: 		Run(); 			break;
		}
	}

	//Get input from the Player
	void GetInput(){
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Input.GetMouseButtonDown(0)){
			if (Physics.Raycast(ray, out rayHit, 100f)){
				if(rayHit.transform.tag == "Tile"){
					if(Vector3.Distance(transform.position, new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z)) > 1){
						targetPosition 		= new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z);
						currentMoveState 	= MoveState.Run;
						ClearPath();
					}
					else{
						currentMoveState = MoveState.Idle;
					}
				}
			}
		}
	}

//	void RotateOverTime(){
//		if (currentMoveState == MoveState.CrouchWalk || currentMoveState == MoveState.CrouchWalk){
//			Quaternion lookRotation = Quaternion.LookRotation (destinationObject.transform.position - transform.position);
//			transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation,  Time.deltaTime);
//		}
//	}

	void Idle(){
		distance = 1000f;
	}

	void CrouchWalk(){
		playerAnimator.SetFloat("animationSpeed", timetest.currentScale * moveSpeed);
		distance 			= Vector3.Distance(transform.position, targetPosition);
		agent.destination 	= targetPosition;

		//agent.speed		= playerAnimator.GetFloat("animationSpeed");
		if(distance > 0.5f){

		}
		else{
			agent.Stop();
			Destroy(destinationObject);
			currentMoveState = MoveState.Idle;
		}
	}

	void Run(){


		PathIndicator();


		float runSpeed = 3.0f;

		distance 			= Vector3.Distance(transform.position, targetPosition);
		agent.destination 	= targetPosition;
		if(distance > 0.5f){

		}
		else{
			//agent.Stop();
			Destroy(destinationObject);
			currentMoveState = MoveState.Idle;
		}
	}
	


	void PathIndicator(){
		if(distance < 0.5f)ClearPath();

		if(!destinationObject)
		destinationObject = Instantiate(Resources.Load("DestinationMarker"), targetPosition, Quaternion.identity) as GameObject;

		if(!pathObject){
			pathObject = Instantiate(Resources.Load("PlayerPathIndicator"), transform.position + (transform.forward * 1f), Quaternion.identity) as GameObject;
			pathObject.GetComponent<PlayerPathIndicator>().targetPosition = targetPosition + new Vector3(0,0f,0);
			pathObject.GetComponent<PlayerPathIndicator>().SetPlayer(gameObject);
		}
	}

	void ClearPath(){
		Destroy(pathObject);
		Destroy(destinationObject);
	}
}
