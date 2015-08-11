using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour {
	
	public Vector2 targetLocation;

	float moveSpeed = 13;
	RectTransform buttonTransform;
	Vector2 startlocation;

	// Use this for initialization
	void Start () {
		buttonTransform = GetComponent<RectTransform>();
		startlocation = buttonTransform.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
		buttonTransform.anchoredPosition = Vector2.Lerp (startlocation, targetLocation, Time.time * moveSpeed );
	}
}
