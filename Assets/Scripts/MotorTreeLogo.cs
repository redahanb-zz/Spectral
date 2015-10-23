using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MotorTreeLogo : MonoBehaviour {

	public RawImage faderImage, engineImage;
	public Camera introCamera;
	bool fadeToBlack = true;
	bool zoomOut = false,
	textVisible = false,
	engineVisible = false,
	copyrightVisible = false;
	public Text motorText, treeText, copyrightText;
	public GameObject musicObject;

	AudioSource engineSource, musicSource;

	AsyncOperation async;

	bool runOnce = false;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		musicSource = musicObject.GetComponent<AudioSource>();
		engineSource = GetComponent<AudioSource>();


		motorText.color = new Color(motorText.color.r, motorText.color.g, motorText.color.b, 0);
		treeText.color = new Color(treeText.color.r, treeText.color.g, treeText.color.b, 0);
		engineImage.color = new Color(engineImage.color.r, engineImage.color.g, engineImage.color.b, 0);

		Invoke("ToggleEngine", 2);

		Invoke("PlayEngine", 2);
		Invoke("PlayMusic", 4);


		Invoke("ToggleFade", 4);
		Invoke("ToggleFade", 9);
		Invoke("ToggleEngine", 9);

		Invoke("ToggleZoomOut", 4);
		Invoke("ToggleText", 6);

		Invoke("ToggleCopyright", 11);
		Invoke("ToggleCopyright", 16);


	}
	
	// Update is called once per frame
	void Update () {

		if(!runOnce){
			StartCoroutine(AsyncLoadLevel());
			Invoke("StartNextScene", 18);
			runOnce = true;
		}

		Fade();
		ZoomOut();
		DisplayText();
		ShowEngine();
		DisplayCopyright();
	}

	void PlayMusic(){
		musicSource.Play();
	}

	void PlayEngine(){
		engineSource.Play();
	}

	void ToggleZoomOut(){
		zoomOut = !zoomOut;
	}

	void ToggleFade(){
		fadeToBlack = !fadeToBlack;
	}

	void ToggleText(){
		textVisible = !textVisible;
	}

	void ToggleEngine(){
		engineVisible = !engineVisible;
	}

	void ToggleCopyright(){
		copyrightVisible = !copyrightVisible;
	}

	void ZoomOut(){
		if(zoomOut){
			introCamera.fieldOfView = Mathf.Lerp(introCamera.fieldOfView, 50, Time.deltaTime);
			introCamera.transform.position = Vector3.Lerp(introCamera.transform.position, new Vector3(0,28,-180), Time.deltaTime);
		}
	}

	void Fade(){
		if(fadeToBlack){
			faderImage.color = Color.Lerp(faderImage.color, new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, 1), Time.deltaTime);
		}
		else{
			faderImage.color = Color.Lerp(faderImage.color, new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, 0), Time.deltaTime * 1.5f);
		}
	}

	void ShowEngine(){
		if(engineVisible)
			engineImage.color = Color.Lerp(engineImage.color, new Color(engineImage.color.r, engineImage.color.g, engineImage.color.b, 1), Time.deltaTime * 0.7f);
		
		else
			engineImage.color = Color.Lerp(engineImage.color, new Color(engineImage.color.r, engineImage.color.g, engineImage.color.b, 0), Time.deltaTime * 2);
	}

	void DisplayText(){
		if(textVisible){
			motorText.color = Color.Lerp(motorText.color, new Color(motorText.color.r, motorText.color.g, motorText.color.b, 1), Time.deltaTime);
			treeText.color = Color.Lerp(treeText.color, new Color(treeText.color.r, treeText.color.g, treeText.color.b, 1), Time.deltaTime * 0.3f);
		}
		else{
			motorText.color = Color.Lerp(motorText.color, new Color(motorText.color.r, motorText.color.g, motorText.color.b, 0), Time.deltaTime);
			treeText.color = Color.Lerp(treeText.color, new Color(treeText.color.r, treeText.color.g, treeText.color.b, 0), Time.deltaTime);
		}
	}

	void DisplayCopyright(){
		if(copyrightVisible){
			copyrightText.color = Color.Lerp(copyrightText.color, new Color(copyrightText.color.r, copyrightText.color.g, copyrightText.color.b, 1), Time.deltaTime * 3);
		}
		else{
			copyrightText.color = Color.Lerp(copyrightText.color, new Color(copyrightText.color.r, copyrightText.color.g, copyrightText.color.b, 0), Time.deltaTime * 3);
		}
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
