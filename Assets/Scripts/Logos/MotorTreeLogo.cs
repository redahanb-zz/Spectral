//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Displays the motor tree logo and copyright when the game starts.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MotorTreeLogo : MonoBehaviour {

	public RawImage 	faderImage,					//image used for fading to black 
						engineImage;				//image of engine
	public Camera 		introCamera;				//main camera component
	private bool 		fadeToBlack = true, 		//determines if screen fades to black
						runOnce = false;			//loads next scene onece
	private bool 		zoomOut = false,			//if true, camera zooms out
						textVisible = false,		//determines if text is visible
						engineVisible = false,		//determines if engine is visible
						copyrightVisible = false;	//determines if copyright is visible
	public Text 		motorText, 					//motor text component
						treeText, 					//tree text component
						copyrightText;				//copyright text component
	public GameObject 	musicObject;

	private AudioSource engineSource, 				//engine audio source
						musicSource;				//music audio source

	private AsyncOperation async;					//AsyncOperation used to determine when scene transition occurs

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

	//Starts playing music clip.
	void PlayMusic(){
		musicSource.Play();
	}

	//Starts playing engine clip.
	void PlayEngine(){
		engineSource.Play();
	}

	//Toggles the camera zoom behaviour.
	void ToggleZoomOut(){
		zoomOut = !zoomOut;
	}

	//Toggles screen fading behaciour.
	void ToggleFade(){
		fadeToBlack = !fadeToBlack;
	}

	//Toggles text visibility.
	void ToggleText(){
		textVisible = !textVisible;
	}

	//Toggles the engine display.
	void ToggleEngine(){
		engineVisible = !engineVisible;
	}

	//Toggles the copyright display.
	void ToggleCopyright(){
		copyrightVisible = !copyrightVisible;
	}

	//Zooms out the camera.
	void ZoomOut(){
		if(zoomOut){
			introCamera.fieldOfView = Mathf.Lerp(introCamera.fieldOfView, 50, Time.deltaTime);
			introCamera.transform.position = Vector3.Lerp(introCamera.transform.position, new Vector3(0,28,-180), Time.deltaTime);
		}
	}

	//Fades the screen.
	void Fade(){
		if(fadeToBlack) faderImage.color = Color.Lerp(faderImage.color, new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, 1), Time.deltaTime);
		else faderImage.color = Color.Lerp(faderImage.color, new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, 0), Time.deltaTime * 1.5f);
	}

	//Shows or hides the engine graphic.
	void ShowEngine(){
		if(engineVisible) engineImage.color = Color.Lerp(engineImage.color, new Color(engineImage.color.r, engineImage.color.g, engineImage.color.b, 1), Time.deltaTime * 0.7f);
		else engineImage.color = Color.Lerp(engineImage.color, new Color(engineImage.color.r, engineImage.color.g, engineImage.color.b, 0), Time.deltaTime * 2);
	}

	//Shows or hides motortree text.
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

	//Shows or hides the copyright.
	void DisplayCopyright(){
		if(copyrightVisible){
			copyrightText.color = Color.Lerp(copyrightText.color, new Color(copyrightText.color.r, copyrightText.color.g, copyrightText.color.b, 1), Time.deltaTime * 3);
		}
		else{
			copyrightText.color = Color.Lerp(copyrightText.color, new Color(copyrightText.color.r, copyrightText.color.g, copyrightText.color.b, 0), Time.deltaTime * 3);
		}
	}

	//Triggers the next scene.
	void StartNextScene(){
		async.allowSceneActivation = true;
	}
	
	//Loads the next scene.
	IEnumerator AsyncLoadLevel() {
		async = Application.LoadLevelAsync("Greenlight Screen");
		async.allowSceneActivation = false;
		yield return async;
		Debug.Log("Loading complete");
	}


}
