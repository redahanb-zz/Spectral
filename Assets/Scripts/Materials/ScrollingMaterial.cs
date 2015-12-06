//Name:			ScrollingMaterial.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Scrolls a materials texture in a specified direction over time.


using UnityEngine;
using System.Collections;

public class ScrollingMaterial : MonoBehaviour {

	private Renderer render;			//The renderer component
	public Vector2 scrollDirection;		//The scroll direction

	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		render.material.mainTextureOffset = render.material.mainTextureOffset + scrollDirection * Time.deltaTime;
	}
}
