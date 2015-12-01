using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScaleRectTransformToScreen : MonoBehaviour {

	RectTransform rTransform;

	public bool scaleEveryFrame = false;

	// Use this for initialization
	void Start () {
		rTransform = GetComponent<RectTransform>();
		rTransform.sizeDelta = new Vector3(Screen.width, Screen.height,0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(scaleEveryFrame)rTransform.sizeDelta = new Vector3(Screen.width, Screen.height,0);
	}
}
