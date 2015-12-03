using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRotateOverTime : MonoBehaviour {
	
	RectTransform rTransform;
	public Vector3 rotateDirection;
	// Use this for initialization
	void Start () {
		rTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		rTransform.localEulerAngles += rotateDirection * Time.deltaTime;
	}
}
