using UnityEngine;
using System.Collections;

public class PlayerTouchMovement : MonoBehaviour {

	GameObject player;

	Ray ray;
	RaycastHit hit;
	Vector3 spawnPosition;
	GameObject markerObject;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		for (var i = 0; i < Input.touchCount; i++) {
			ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
			if (Input.GetTouch(i).phase == TouchPhase.Began){
				if (Physics.Raycast(ray, out hit)){
					if(hit.transform.tag == "Tile"){
						spawnPosition = hit.point;
						if(player)player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
					}
				}

			}
		}

	}
}
