using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingBackgroundUI : MonoBehaviour {

	RawImage rImage;
	RectTransform rTransform;
	float xPos = 0;
	// Use this for initialization
	void Start () {
		rImage = GetComponent<RawImage>();
		rTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		xPos = xPos -0.001f;

		rTransform.eulerAngles += new Vector3(0,0,0.025f);

		rImage.uvRect = new Rect(xPos,0,rImage.uvRect.width,rImage.uvRect.height);
	}
}
