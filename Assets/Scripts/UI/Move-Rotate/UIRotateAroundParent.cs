using UnityEngine;
using System.Collections;

public class UIRotateAroundParent : MonoBehaviour {

	float rotateSpeed = 120;
	public bool rotateRight = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(rotateRight) transform.RotateAround(transform.parent.position, Vector3.forward, rotateSpeed * 0.01f);
		else transform.RotateAround(transform.parent.position, -Vector3.forward, rotateSpeed * 0.01f);
	}
}
