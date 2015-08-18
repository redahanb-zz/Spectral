using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FitRawImageToScreen : MonoBehaviour {

	RectTransform rT;

	// Use this for initialization
	void Start () {
		rT = GetComponent<RectTransform>();
		rT.sizeDelta = new Vector2(Screen.width, Screen.height);
	}	
}
