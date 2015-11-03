using UnityEngine;
using System.Collections;

public class FootstepFX : MonoBehaviour {

	Vector3 drawLoc;
	public bool enemyInRange;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playEcho() {
		drawLoc = transform.position;
		drawLoc.y += 0.23f;
		if(enemyInRange){
			Instantiate (Resources.Load("FootfallFX"), drawLoc, Quaternion.Euler(90,0,0));
		}
	}
}
