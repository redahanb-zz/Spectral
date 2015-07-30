using UnityEngine;
using System.Collections;

public class PlayerNavmeshTest : MonoBehaviour {

	TimeScaler 		timeScale;

	Animator 		playerAnimator;
	NavMeshAgent 	agent;
	NavMeshPath		path;

	Ray 			ray;
	RaycastHit 		rayHit;

	int 			currentPathIndex = 1;

	float 			distance = 0f,
					moveSpeed = 1f,
					currentSpeed = 0f,
					maxSpeed = 7.0f,
					turnDistance = 100, targetSpeed = 0;

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
		agent.SetDestination(transform.position);
		timeScale 		= GameObject.Find("Time Manager").GetComponent<TimeScaler>();
	}
	
	// Update is called once per frame
	void Update () {
		hasPath = agent.hasPath;
		GetInput();
		MoveStateManager();

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
				//print("Tag: " +rayHit.transform.tag + "   Name: " +rayHit.transform.name);
				if(rayHit.transform.tag == "Tile"){

					//print("" +Vector3.Distance(transform.position, new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z)));
					if(Vector3.Distance(transform.position, new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z)) > 1){
						ClearPath();
						targetPosition 		= new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z);
						currentMoveState 	= MoveState.Run;
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
		playerAnimator.SetFloat("animationSpeed", timeScale.currentScale * moveSpeed);
		distance 			= Vector3.Distance(transform.position, targetPosition);

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



		//float runSpeed = 3.0f;
		agent.SetDestination(targetPosition);

		distance = Vector3.Distance(transform.position, targetPosition);


		PathIndicator();
		if(distance > 0.5f){

		}
		else{
			Destroy(destinationObject);
			currentMoveState = MoveState.Idle;
		}
	}

	void SetSpeed(){
		turnDistance = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
		if(turnDistance > 3.5f)targetSpeed = maxSpeed;
		else if(transform.forward != (path.corners[currentPathIndex] - transform.position)) targetSpeed = 0.5f;
		else targetSpeed = 0.5f;
		
		print("Distance: " +turnDistance +"   Speed: " +currentSpeed +"   Target Speed: " +targetSpeed);
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 3);
		playerAnimator.SetFloat("movementSpeed" , currentSpeed);

		//transform.LookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
		SmoothLookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
	}

	void SmoothLookAt(Vector3 targetPosition){
		Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 7.5f);
	}

	void PathIndicator(){

		//if(distance < 0.5f)ClearPath();

		GetPath();
		SetSpeed();





		if(!timeScale.timeSlowed){
			ClearPath();
		}

		if(!destinationObject && timeScale.timeSlowed)
			destinationObject = Instantiate(Resources.Load("DestinationMarker"), targetPosition, Quaternion.identity) as GameObject;

		if(!pathObject && timeScale.timeSlowed){
			pathObject = Instantiate(Resources.Load("PathIndicator"), transform.position + new Vector3(0,0.01f,0) + (transform.forward * 0.4f), Quaternion.identity) as GameObject;
			agent.SetDestination(targetPosition);
			pathObject.transform.eulerAngles = transform.eulerAngles;
			pathObject.GetComponent<PathIndicator>().SetPath(agent.path);
		}
	}

	void GetPath(){
		currentPathIndex = 1;
		path = agent.path;
	}

	void ClearPath(){
		Destroy(pathObject);
		Destroy(destinationObject);
	}
}
