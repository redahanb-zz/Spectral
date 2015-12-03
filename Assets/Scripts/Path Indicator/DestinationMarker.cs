using UnityEngine;
using System.Collections;

public class DestinationMarker : MonoBehaviour {

	Renderer r;
	Vector3 visibleScale, hiddenScale;
	bool markerVisible = true;

	float scaleSpeed = 2;

	// Use this for initialization
	void Start () {
		visibleScale = transform.localScale;
		hiddenScale = new Vector3(0,0,0);
		transform.localScale = hiddenScale;

		r = GetComponent<Renderer>();
		//r.material.color = new Color(r.material.color.a, r.material.color.g, r.material.color.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(markerVisible){
			transform.localScale = Vector3.Lerp(transform.localScale, visibleScale, Time.deltaTime * scaleSpeed);
		}
		else{
			//print("Hide: " +transform.localScale.x);
			transform.localScale = Vector3.Lerp(transform.localScale, hiddenScale, Time.deltaTime * scaleSpeed * 1.6f);
			if(transform.localScale.x < 0.01f)			Destroy(gameObject);

		}

		//r.material.color = Color.Lerp(r.material.color, new Color(r.material.color.a, r.material.color.g, r.material.color.b, 1), 0.06f);
	}

	public void RemoveMarker(){
		markerVisible = false;
	}
}
