using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {

	public Color objectColor;
	Renderer objectRenderer;


	Vector3 objectPosition, startingPosition;

	public bool useColor = true;
	bool blackAndWhite = true;

	// Use this for initialization
	void Start () {

		objectPosition = transform.position;
		startingPosition = transform.position + new Vector3(0,-10, 0);
		transform.position = startingPosition;

		objectRenderer = GetComponent<Renderer>();

		if(objectColor == Color.black)objectColor = Color.red;

		//Invoke("ToggleColour", 3);
	}

	void ToggleColour(){
		blackAndWhite = !blackAndWhite;
	}
	
	// Update is called once per frame
	void Update () {

		if(Vector3.Distance(transform.position, objectPosition) < 0.01f)ToggleColour();

		else{
			transform.position = Vector3.Lerp(transform.position, objectPosition, Time.deltaTime * 3);
		}

		if(useColor){
			if(!blackAndWhite){
				objectRenderer.material.color = Color.Lerp(objectRenderer.material.color, objectColor, Time.deltaTime * 3);
			}
			else{
				objectRenderer.material.color = Color.Lerp(objectRenderer.material.color, Color.white, Time.deltaTime * 3);
			}
		}
	}
}
