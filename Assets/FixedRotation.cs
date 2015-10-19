using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour {

	Quaternion rotation;
	Vector3 postion;
	void Awake()
	{
		rotation = transform.rotation;
		postion  = transform.position;
	}
	void LateUpdate()
	{
		transform.rotation = rotation;
		transform.position = postion;
	}
}
