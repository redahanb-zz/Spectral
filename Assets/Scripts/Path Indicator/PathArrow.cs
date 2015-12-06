//Name:			PathArrow.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This fades in and out an arrow object that is used to indicate the current path.

using UnityEngine;
using System.Collections;

public class PathArrow : MonoBehaviour {
	private Renderer 	arrowRenderer;		//renderer component of arrow
	private bool 		visible = false, 	//indicates arrow is visible
						ready = false;		//indicates arrow is ready to fade
	private Ray 		ray;				//Raycast ray
	private RaycastHit 	hit;				//Raycast hit

	// Use this for initialization
	void Start () {
		arrowRenderer = GetComponent<Renderer>();
		arrowRenderer.material.color = new Color(0.5f,0.5f,0.5f,0);
		CheckIfVisible();
	}

	//Checks if the arrow has become visible
	void CheckIfVisible(){
		ray = Camera.main.ScreenPointToRay(transform.position);

		Vector3 heading = transform.position - Camera.main.transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance; 

		if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 100.0F)){
			if(hit.transform.name == transform.name) arrowRenderer.material = Resources.Load("Chevron Visible") as Material;
			else arrowRenderer.material = Resources.Load("Chevron Visible") as Material;
		}
		ready = true;
	}

	// Update is called once per frame
	void Update () {
		if(ready){
			if(visible){
				arrowRenderer.material.color = Color.Lerp(arrowRenderer.material.color, new Color(0.5f,0.5f,0.5f,0), 0.05f);
				if(arrowRenderer.material.color.a < 0.1f)Destroy(gameObject);
			}
			else{
				arrowRenderer.material.color = Color.Lerp(arrowRenderer.material.color, new Color(0.5f,0.5f,0.5f,1), 0.25f);
				if(arrowRenderer.material.color.a > 0.95f)visible = true;
			}
		}
	}
}
