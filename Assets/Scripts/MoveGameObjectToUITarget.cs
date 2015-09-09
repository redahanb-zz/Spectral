using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveGameObjectToUITarget : MonoBehaviour {

	public Vector3 objectPosition, targetObjectPosition, uiElementPosition;

	public GameObject targetUIElement;

	RectTransform rtransform;

	// Use this for initialization
	void Start () {
		rtransform = targetUIElement.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		uiElementPosition = rtransform.position;
		transform.position = Camera.main.ScreenToWorldPoint(uiElementPosition);
	}
}
