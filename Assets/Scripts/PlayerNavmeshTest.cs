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
					currentSpeed = 0f,
					turnDistance = 100, 
					targetSpeed = 0,
					runSpeed = 20.0f, 
					walkSpeed = 0.5f,
					sneakSpeed = 0.5f,
					stopSpeed = 0.1f;

	Vector3 		targetPosition;

	GameObject 		pathObject, 
					destinationObject;

	bool 			canMove = false,
					hasPath = false,
					doubleTap = false;

	enum 			MoveState{ Idle, Sneak, Run};
	MoveState 		currentMoveState = MoveState.Idle;

	float 			customDeltaTime,
					timeSinceLastClick = 0;

	// Use this for initialization
	void Start () {
		customDeltaTime = Time.deltaTime;
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
			case MoveState.Sneak: 		Sneak(); 		break;
			case MoveState.Run: 		Run(); 			break;
		}

		print("Double Tap: " +doubleTap + "   MoveState: (" +(int)currentMoveState +") " +currentMoveState +"   AnimState: " +playerAnimator.GetFloat("moveState"));
	}

	//Get input from the Player
	void GetInput(){
		timeSinceLastClick = timeSinceLastClick + customDeltaTime;

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Input.GetMouseButtonDown(0) && timeScale.timeSlowed){
			if(timeSinceLastClick < 1)
				doubleTap = true;
			else 
				doubleTap = false;
			timeSinceLastClick = 0;

			//Mouse Input
			if (Physics.Raycast(ray, out rayHit, 100f)){
				if(rayHit.transform.tag == "Tile"){
					if(Vector3.Distance(transform.position, new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z)) > 1){
						ClearPath();
						targetPosition 		= new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z);
						if(doubleTap) 
							currentMoveState 	= MoveState.Run;
						else 
							currentMoveState 	= MoveState.Sneak;
					}
					else{
						currentMoveState = MoveState.Idle;
					}
				}
			}
		}

		//Touch Input
		for (var i = 0; i < Input.touchCount; i++) {

			if ((Input.GetTouch(i).phase == TouchPhase.Began) && (Input.touchCount < 2) && (timeSinceLastClick > 0.3f)){
				ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);

				if(timeSinceLastClick < 1)
					doubleTap = true;
				else 
					doubleTap = false;
				timeSinceLastClick = 0;

				if (Physics.Raycast(ray, out rayHit)){
					if(rayHit.transform.tag == "Tile"){
						if(Vector3.Distance(transform.position, new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z)) > 1){
							ClearPath();
							targetPosition 		= new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z);
							if(doubleTap) 
								currentMoveState 	= MoveState.Run;
							else 
								currentMoveState 	= MoveState.Sneak;
						}
						else{
							currentMoveState = MoveState.Idle;
						}
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
	

	void Sneak(){
		agent.SetDestination(targetPosition);
		distance = Vector3.Distance(transform.position, targetPosition);
		GetPath();
		SetSneakSpeed();
		PathIndicator();
		
		if(distance < 0.5f){
			Destroy(destinationObject);
			currentMoveState = MoveState.Idle;
		}
	}

	void Run(){
		agent.SetDestination(targetPosition);
		distance = Vector3.Distance(transform.position, targetPosition);
		GetPath();
		SetRunSpeed();
		PathIndicator();

		if(distance < 0.5f){
			Destroy(destinationObject);
			currentMoveState = MoveState.Idle;
		}
	}

	void SetRunSpeed(){

		Vector3 	head = path.corners[currentPathIndex] - transform.position; 
		float		dist = head.magnitude;
		Vector3 	dir  = head/dist;


		if(path.corners.Length > 2)turnDistance = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
		else turnDistance = Vector3.Distance(transform.position, targetPosition);
		if(turnDistance > 4.5f){targetSpeed = runSpeed;   agent.speed = 4;}

		else{ targetSpeed = walkSpeed;   agent.speed = 2;}

		if(transform.forward != dir){ targetSpeed = walkSpeed;  agent.speed = 1;}

		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 3);
		playerAnimator.SetFloat("movementSpeed" , currentSpeed);

		if(path.corners.Length > 2)SmoothLookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
		else SmoothLookAt(targetPosition);
	}

	void SetSneakSpeed(){
		//Heading, distance and direction of next corner
		Vector3 	head;
		if(path.corners.Length > 2)	head = path.corners[currentPathIndex] - transform.position; 
		else head = targetPosition - transform.position; 
		float		dist = head.magnitude;
		Vector3 	dir  = head/dist;

		//print("Distance: " +turnDistance +"   Speed: " +currentSpeed +"   Target Speed: " +targetSpeed +   "   Agent Speed: " +agent.speed);


		//Get distance of next corner
		if(path.corners.Length > 2)turnDistance = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
		else turnDistance = Vector3.Distance(transform.position, targetPosition);

		//If near corner, walk.	Otherwise, run.
		if(turnDistance > 0.7f){targetSpeed = sneakSpeed;   agent.speed = 2;}
		else{ targetSpeed = stopSpeed;   agent.speed = 1;}

		//If not facing direction of next corner, walk
		if(transform.forward != dir){ targetSpeed = walkSpeed;  agent.speed = 1;}

		//Update current speed
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 3);
		playerAnimator.SetFloat("movementSpeed" , currentSpeed);

		//Look at next corner
		if(path.corners.Length > 2)SmoothLookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
		else SmoothLookAt(targetPosition);
	}

	void SmoothLookAt(Vector3 targetPosition){
		Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8.5f);
	}

	void PathIndicator(){


		if(!timeScale.timeSlowed){
			ClearPath();
		}

		if(!destinationObject && timeScale.timeSlowed)
			destinationObject = Instantiate(Resources.Load("DestinationMarker"), targetPosition, Quaternion.identity) as GameObject;

		if(!pathObject && timeScale.timeSlowed && distance > 1){
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
