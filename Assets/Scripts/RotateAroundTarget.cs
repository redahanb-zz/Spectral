using UnityEngine;
using System.Collections;

public class RotateAroundTarget : MonoBehaviour {
	public Transform target;
	public float speed = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(target.position, Vector3.up, Time.deltaTime * speed);
	}
}
