using UnityEngine;
using System.Collections;

public class PlayerTouchController : MonoBehaviour {


	// Use this for initialization
	void Start () {
		PlayerTouchMovement[] touchObjs = FindObjectsOfType(typeof(PlayerTouchMovement)) as PlayerTouchMovement[];
		foreach (PlayerTouchMovement t in touchObjs) {
			print("TOUCH: " +t.gameObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
