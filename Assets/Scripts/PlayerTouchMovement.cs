using UnityEngine;
using System.Collections;

public class PlayerTouchMovement : MonoBehaviour {

	GameObject destinationMarker;

	Ray ray;
	RaycastHit hit;
	Vector3 spawnPosition;
	GameObject markerObject;
	// Use this for initialization
	void Start () {
		destinationMarker = Resources.Load("DestinationMarker") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {


		for (var i = 0; i < Input.touchCount; i++) {
			ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
			if (Input.GetTouch(i).phase == TouchPhase.Began){
				if (Physics.Raycast(ray, out hit)){
					spawnPosition = hit.point;
					if(markerObject)markerObject.transform.position = spawnPosition;
					else markerObject = Instantiate(destinationMarker, spawnPosition, transform.rotation) as GameObject;
				}

			}
		}

	}
}
