using UnityEngine;
using System.Collections;

public class FootstepFX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playEcho() {
		Vector3 drawLoc = transform.position;
		drawLoc.y += 0.01f;
		Instantiate (Resources.Load("FootfallFX"), drawLoc, Quaternion.Euler(90,0,0));
	}
}
