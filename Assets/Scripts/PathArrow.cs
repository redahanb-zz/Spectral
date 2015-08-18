using UnityEngine;
using System.Collections;

public class PathArrow : MonoBehaviour {
	Renderer arrowRenderer;
	bool visible = false;
	Ray ray;
	RaycastHit hit;
	bool ready = false;

	// Use this for initialization
	void Start () {

		arrowRenderer = GetComponent<Renderer>();
		arrowRenderer.material.color = new Color(0,0,0,0);
		Invoke("CheckIfVisible", 0.04f);
		//CheckIfVisible();
	}

	void CheckIfVisible(){
		ray = Camera.main.ScreenPointToRay(transform.position);

		Vector3 heading = transform.position - Camera.main.transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance; 


		if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 100.0F)){
			print(hit.transform.name +" : " + transform.name);
			print(hit.transform);
			if(hit.transform.name == transform.name){
				//Arrow is visible
				print("VISIBLE");
				arrowRenderer.material = Resources.Load("Chevron Visible") as Material;
			}
			else{
				//Arrow is hidden
				print("HIDDEN");
				arrowRenderer.material = Resources.Load("Chevron Hidden") as Material;
			}
		}
		ready = true;
	}

	// Update is called once per frame
	void Update () {
		if(ready){
			Debug.DrawRay(Camera.main.transform.position, hit.point, Color.yellow);
			if(visible){
				arrowRenderer.material.color = Color.Lerp(arrowRenderer.material.color, new Color(1,1,1,0), 0.05f);
				if(arrowRenderer.material.color.a < 0.1f)Destroy(gameObject);
			}
			else{
				arrowRenderer.material.color = Color.Lerp(arrowRenderer.material.color, new Color(1,1,1,1), 0.25f);
				if(arrowRenderer.material.color.a > 0.95f)visible = true;
			}
		}
	}
}
