using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenOverlayPattern : MonoBehaviour {

	RectTransform rTrans;
	// Use this for initialization
	void Start () {
		rTrans = transform.GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
