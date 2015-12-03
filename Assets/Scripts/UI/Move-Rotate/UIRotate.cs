using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRotate : MonoBehaviour {

	RectTransform rTransform;
	public Vector3 rotateDirection;
	RectTransform[] childRectTransforms;

	// Use this for initialization
	void Start () {
//		if(transform.childCount > 0)
//			childRectTransforms = GetComponentsInChildren<RectTransform>();
		rTransform = GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {

			rTransform.localEulerAngles += rotateDirection;

	}

	void LateUpdate(){
//		if(transform.childCount > 0)
//		{
//			foreach(RectTransform t in childRectTransforms){
//				print(t.eulerAngles);
//				if(t != rTransform) t.localEulerAngles = new Vector3(0,0,0);
//			}
//			
//		}
	}
}
