using UnityEngine;
using System.Collections;

public class FootstepFX : MonoBehaviour {

	Vector3 drawLoc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playEcho() {
		drawLoc = transform.position;
		drawLoc.y += 0.23f;
		Instantiate (Resources.Load("FootfallFX"), drawLoc, Quaternion.Euler(90,0,0));
	}
}
