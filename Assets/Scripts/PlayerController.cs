using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
	
	EventSystem eventSystem;
	
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
					doubleTap = false,
					performAction = false;

	public bool 	isVisible = true,
					isBlending = false;
	
	enum 			MoveState{ Idle, Sneak, Run, Blend_Stand, Blend_Prone};
	MoveState 		currentMoveState = MoveState.Idle;
	
	float 			customDeltaTime,
	timeSinceLastClick = 0;

	Renderer		playerRenderer;

	public Color targetcolor = Color.grey;

	
	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		customDeltaTime = Time.deltaTime;
		playerAnimator 	= GetComponent<Animator>();
		playerRenderer 	= transform.Find("Model").GetComponent<Renderer>();
		playerRenderer.material.color = Color.green;
		agent 			= GetComponent<NavMeshAgent>();
		agent.SetDestination(transform.position);
		timeScale 		= GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		Invoke("ToggleCanMove", 0.1f);
		
	}
	
	public void ToggleCanMove(){
		canMove = !canMove;
	}
	
	// Update is called once per frame
	void Update () {
		hasPath = agent.hasPath;
		if(canMove){
			GetInput();
			MoveStateManager();
		}
		else StopMoving();

		playerRenderer.material.color = Color.Lerp(playerRenderer.material.color, targetcolor, 2*Time.deltaTime);
	}

	//Assigns action per state and sets the animator move state
	void MoveStateManager(){
		playerAnimator.SetInteger("moveState", (int)currentMoveState);
		
		switch(currentMoveState){
		case MoveState.Idle: 			Idle(); 				isBlending = false; isVisible = true;	break;
		case MoveState.Sneak: 			Sneak(); 				isBlending = false; isVisible = true;	break;
		case MoveState.Run: 			Run(); 					isBlending = false; isVisible = true;	break;
		case MoveState.Blend_Stand: 	BlendWhileStanding(); 	isBlending = true; break;
		}
	}
	
	//Get input from the Player
	void GetInput(){
		timeSinceLastClick = timeSinceLastClick + customDeltaTime;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//print( eventSystem.IsPointerOverGameObject());
		if(Input.GetMouseButtonDown(0) && timeScale.timeSlowed ){
			
			//Check for DOuble Tap
			if(timeSinceLastClick < 1)
				doubleTap = true;
			else 
				doubleTap = false;
			timeSinceLastClick = 0;
			
			if(!eventSystem.IsPointerOverGameObject())
			if (Physics.Raycast(ray, out rayHit, 100f)){
				print(rayHit.transform);
				switch(rayHit.transform.tag){
				case "Tile" : 
					//print("Floor");
					SetMovement(rayHit.transform, rayHit.point);
					performAction = false;
					break;
				case "Blend Surface" :
					//print("Blend Surface");
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.35f));
					performAction = true;
					break;
				}
				
				
			}
		}
		
		//Touch Input
		for (var i = 0; i < Input.touchCount; i++) {
			
			if ((Input.GetTouch(i).phase == TouchPhase.Began) && (Input.touchCount < 2) && (timeSinceLastClick > 0.3f) && !eventSystem.IsPointerOverGameObject() ){
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
	
	
	void SetMovement(Transform t, Vector3 v){
			if(Vector3.Distance(transform.position, new Vector3(v.x, transform.position.y, v.z)) > 1){
				ClearPath();
				targetPosition 		= new Vector3(v.x, transform.position.y, v.z);
				if(doubleTap) 
					currentMoveState 	= MoveState.Run;
				else 
					currentMoveState 	= MoveState.Sneak;
			}
			else{
				currentMoveState = MoveState.Idle;
			}
		//}
	}
	
	void Idle(){
		distance = 1000f;
	}

	void BlendWhileStanding(){
		if(isBlending){
			print ("Player color: " + playerRenderer.material.color);
			print ("Wall color: " + rayHit.transform.GetComponent<Renderer>().material.color);
			if(playerRenderer.material.color == rayHit.transform.GetComponent<Renderer>().material.color){
				print("Player cannot be seen.");
				isVisible = false;
			}
			else{
				isVisible = true;
				print("Player is visible to enemies.");
			}
		}
	}
	
	
	void Sneak(){
		agent.SetDestination(targetPosition);
		distance = Vector3.Distance(transform.position, targetPosition);
		GetPath();
		SetSneakSpeed();
		PathIndicator();
		
		if(distance < 0.5f){
			if(performAction)
				PerformAnAction();
			else currentMoveState = MoveState.Idle;
			Destroy(destinationObject);
		}
	}
	
	void Run(){
		agent.SetDestination(targetPosition);
		distance = Vector3.Distance(transform.position, targetPosition);
		GetPath();
		SetRunSpeed();
		PathIndicator();
		
		if(distance < 0.5f){
			if(performAction)
				PerformAnAction();
			else currentMoveState = MoveState.Idle;
			Destroy(destinationObject);
		}
	}
	
	void SetRunSpeed(){
		Vector3 	head;
		if(path.corners.Length > 2) head = path.corners[currentPathIndex] - transform.position; 
		else head = targetPosition - transform.position; 
		float		dist = head.magnitude;
		Vector3 	dir  = head/dist;
		
		
		if(path.corners.Length > 2)turnDistance = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
		else turnDistance = Vector3.Distance(transform.position, targetPosition);
		if(turnDistance > 4.5f){targetSpeed = runSpeed;   agent.speed = 4;}
		
		else{ targetSpeed = walkSpeed;   agent.speed = 2;}
		
		//if(transform.forward != dir){ targetSpeed = walkSpeed;  agent.speed = 1;}
		
		Quaternion q = Quaternion.LookRotation(head);
		Vector3 v3Euler = q.eulerAngles;
		
		float rotDistance = Vector3.Distance(transform.eulerAngles, v3Euler);
		//print(rotDistance);
		//if(transform.eulerAngles == v3Euler)print("ASD");
		
		if(rotDistance > 5){ targetSpeed = walkSpeed;  agent.speed = 1;}
		//print(transform.eulerAngles + " : " + v3Euler);
		
		
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
		if(turnDistance > 0.5f){targetSpeed = sneakSpeed;   agent.speed = 2;}
		else{ targetSpeed = stopSpeed;   agent.speed = 1;}
		
		//If not facing direction of next corner, walk
		if(transform.forward != dir){ targetSpeed = walkSpeed;  agent.speed = 1;}
		
		//Update current speed
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5);
		playerAnimator.SetFloat("movementSpeed" , currentSpeed);
		
		//Look at next corner
		if(path.corners.Length > 2)SmoothLookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
		else SmoothLookAt(targetPosition);
	}
	
	void SmoothLookAt(Vector3 targetPosition){
		Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8.5f);
	}
	
	public void StopMoving(){
		currentMoveState = MoveState.Idle;
	}
	
	void PathIndicator(){
		
		
		if(!timeScale.timeSlowed){
			ClearPath();
		}
		
		if(!destinationObject && timeScale.timeSlowed)
			destinationObject = Instantiate(Resources.Load("DestinationMarker"), targetPosition, Quaternion.identity) as GameObject;
		
		if(!pathObject && timeScale.timeSlowed && distance > 1){
			pathObject = Instantiate(Resources.Load("PathIndicator"), transform.position + new Vector3(0,0.01f,0) + (transform.forward * 1), Quaternion.identity) as GameObject;
			agent.SetDestination(targetPosition);
			pathObject.transform.eulerAngles = transform.eulerAngles;
			pathObject.GetComponent<PathIndicator>().SetPath(agent.path);
		}
	}

	void PerformAnAction(){
		if(rayHit.transform.tag == "Blend Surface"){
			transform.forward = -rayHit.transform.forward;
			isBlending = true;
			currentMoveState = MoveState.Blend_Stand;
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
