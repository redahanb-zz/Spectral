using UnityEngine;
using System.Collections;

public class UpgradeTree : MonoBehaviour {

	Vector3 mouseStartPosition, currentMousePosition, positionOffset, offsetFromCenter;
	RectTransform rTransform;
	// Use this for initialization
	void Start () {
		rTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		//print(mouseStartPosition + "  :  " +currentMousePosition + "  :  " +positionOffset);
	}

	public void SetOffsetPosition(){
		mouseStartPosition = Input.mousePosition;
		offsetFromCenter = rTransform.localPosition - Input.mousePosition;
	}

	public void OnDrag(){
		currentMousePosition = Input.mousePosition;
		//positionOffset = currentMousePosition - mouseStartPosition;
		rTransform.localPosition = Input.mousePosition + offsetFromCenter;
	}
}
