using UnityEngine;
using System.Collections;

public class ChangeToTargetMaterial : MonoBehaviour {

	public float delay = 0;

	public Material targetMaterial;

	bool changeMaterial = false;

	Renderer objRenderer;
	// Use this for initialization
	void Start () {
		objRenderer = GetComponent<Renderer>();
		Invoke("ToggleChange", delay);
	}

	void ToggleChange(){
		changeMaterial = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(changeMaterial){
			objRenderer.material.Lerp(objRenderer.material, targetMaterial, Time.deltaTime);
		}
	}
}
