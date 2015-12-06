using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingCamera : MonoBehaviour {

	public Transform lookAtTarget;
	Vector2 mouseStartPos, mouseCurrentPos;
	float diff;
	bool mouseDown = false;
	public bool loadScreenVisible = true;

	bool autoRotate = true;
	bool zoomIn = true;
	Camera cam;

	AsyncOperation async;

	RawImage faderImage;

	float rate;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		faderImage = GameObject.Find("Fader").GetComponent<RawImage>();
		faderImage.color = Color.Lerp(faderImage.color, new Color(0,0,0,1), Time.deltaTime/12);
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		Fade();
		CalculateMouseDrag();
		Rotate();
		AutoZoom();
		transform.LookAt(lookAtTarget);
	}

	void CalculateMouseDrag(){
		if(Input.GetMouseButtonDown(0)){
			mouseDown = true; 
			MouseClick();
		}
		//if(Input.GetMouseButton)mouseDown = true;
		if(Input.GetMouseButtonUp(0))mouseDown = false;


		if(mouseDown){
			mouseCurrentPos = Input.mousePosition;
			diff = mouseCurrentPos.x - mouseStartPos.x;
		}
		else{
			//diff = 0;
			if(diff > 0) diff -= (rate/50);
			else if(diff < 0) diff += (rate/50);

		}




		//print("START: " +mouseStartPos + " CURRENT: " +mouseCurrentPos + " DIFF: " +diff);
	}

	void AutoZoom(){
		if(zoomIn){ 
			if(cam.fieldOfView > 50) cam.fieldOfView -= 0.1f; 
			else zoomIn = false;
		}
		else{
			if(zoomIn) if(cam.fieldOfView < 70) cam.fieldOfView += 0.1f; 
			else zoomIn = true;
		}
	}

	void Rotate(){
		if(autoRotate){
			transform.RotateAround(lookAtTarget.position, -Vector3.up, 10 * Time.deltaTime);	

		}
		else{
			if(diff < 0) rate = diff * -1;
			else rate = diff;
			
			if(diff < -0.1f){
				transform.RotateAround(lookAtTarget.position, Vector3.up, rate * Time.deltaTime);	
			}
			else if (diff > 0.1f){
				transform.RotateAround(lookAtTarget.position, -Vector3.up, rate * Time.deltaTime);	
			}
			else{
				
			}
		}
	}


	void Fade(){
		if(loadScreenVisible){
			faderImage.color = Color.Lerp(faderImage.color, new Color(0,0,0,0), Time.deltaTime);
		}
		else{
			faderImage.color = Color.Lerp(faderImage.color, new Color(0,0,0,1), Time.deltaTime);
		}
	}

	public void MouseClick(){
		autoRotate = false;
		mouseStartPos = Input.mousePosition;
		mouseDown = true;
	}

	public void MouseReleased(){
		mouseDown = false;
	}

	void StartNextScene(){
		async.allowSceneActivation = true;
	}

	IEnumerator AsyncLoadLevel() {
		async = Application.LoadLevelAsync("Restore Point");
		async.allowSceneActivation = false;
		yield return async;
		Debug.Log("Loading complete");
	}
}
