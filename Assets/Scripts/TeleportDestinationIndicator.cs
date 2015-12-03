using UnityEngine;
using System.Collections;

public class TeleportDestinationIndicator : MonoBehaviour {

	Renderer indicatorRenderer;
	Color startingColor;
	Vector3 startingPos;

	// Use this for initialization
	void Start () {
		indicatorRenderer = GetComponent<Renderer>();
		startingColor = indicatorRenderer.material.color;
		startingPos = transform.position;
		ResetIndicator();
		InvokeRepeating("ResetIndicator", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += new Vector3(0.003f,0.003f,0.003f);
		transform.position += new Vector3(0,0.004f,0);
		indicatorRenderer.material.color = Color.Lerp(indicatorRenderer.material.color, new Color(startingColor.r,startingColor.g,startingColor.b,0), Time.deltaTime * 3);
	}

	void ResetIndicator(){
		transform.localScale = new Vector3(0,0,0);
		transform.position = startingPos;
		indicatorRenderer.material.color = startingColor;
	}
}
