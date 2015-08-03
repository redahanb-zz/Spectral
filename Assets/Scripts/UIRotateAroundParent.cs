using UnityEngine;
using System.Collections;

public class UIRotateAroundParent : MonoBehaviour {

	float rotateSpeed = 120;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(transform.parent.position, Vector3.forward, rotateSpeed * 0.01f);
	}
}
