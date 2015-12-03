using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeReactionTime : MonoBehaviour {
	
	public 	bool 	timescaleVisible = false;
	public 	int 	currentTimescale = 5;
	
	private int 	currentIndex = 0, 
	nextIndex = 0;
	
	private bool 	fadeIn = true;
	
	public bool		displayPanel = false;
	
	private Color 	activeColor, 
	inactiveColor, 
	nextUpgradeColorVisible, 
	nextUpgradeColorTransparent;
	
	private Vector3 hiddenPos, 
	visiblePos;
	
	private 		RawImage[] timescaleImages;
	private 		RectTransform timescaleInfoTransform;
	
	
	Image			timescaleButtonImage;
	
	Color		buttonTargetColor = Color.white;
	// Use this for initialization
	void Start () {
		
		timescaleButtonImage = GameObject.Find("Timescale Button").GetComponent<Image>();
		buttonTargetColor = Color.white;
		timescaleInfoTransform = transform.GetComponent<RectTransform>();
		
		hiddenPos = timescaleInfoTransform.position;
		visiblePos = timescaleInfoTransform.position + new Vector3(340,0,0);
		timescaleInfoTransform.position = hiddenPos;
		
		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);
		
		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);
		
		timescaleImages = new RawImage[9];
		timescaleImages[0] = transform.Find("Current Timescale").Find("Timescale Bar").Find("1").GetComponent<RawImage>();
		timescaleImages[1] = transform.Find("Current Timescale").Find("Timescale Bar").Find("2").GetComponent<RawImage>();
		timescaleImages[2] = transform.Find("Current Timescale").Find("Timescale Bar").Find("3").GetComponent<RawImage>();
		timescaleImages[3] = transform.Find("Current Timescale").Find("Timescale Bar").Find("4").GetComponent<RawImage>();
		timescaleImages[4] = transform.Find("Current Timescale").Find("Timescale Bar").Find("5").GetComponent<RawImage>();
		timescaleImages[5] = transform.Find("Current Timescale").Find("Timescale Bar").Find("6").GetComponent<RawImage>();
		timescaleImages[6] = transform.Find("Current Timescale").Find("Timescale Bar").Find("7").GetComponent<RawImage>();
		timescaleImages[7] = transform.Find("Current Timescale").Find("Timescale Bar").Find("8").GetComponent<RawImage>();
		timescaleImages[8] = transform.Find("Current Timescale").Find("Timescale Bar").Find("9").GetComponent<RawImage>();
		
		ClearTimescale();
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		MovePanel();
		if(Vector3.Distance(timescaleInfoTransform.position,visiblePos) < 5){
			FillTimescale();
			timescaleVisible = true;
		}
		if(Vector3.Distance(timescaleInfoTransform.position,visiblePos) > 330){
			ClearTimescale();
			timescaleVisible = false;
		}
		
		//Debug Input
		//if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
		//if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;
		
	}
	
	void MovePanel(){
		if(displayPanel){
			timescaleInfoTransform.position = Vector3.Lerp(timescaleInfoTransform.position, visiblePos, 0.1f);
			buttonTargetColor = Color.green;
		}
		else{
			timescaleInfoTransform.position = Vector3.Lerp(timescaleInfoTransform.position, hiddenPos, 0.1f);
			buttonTargetColor = Color.white;
		}
		timescaleButtonImage.color = Color.Lerp(timescaleButtonImage.color,buttonTargetColor, Time.deltaTime * 5);
	}
	
	void FillTimescale(){
		if(timescaleImages[currentIndex].color.a < 0.95f){
			timescaleImages[currentIndex].color = Color.Lerp(timescaleImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (currentTimescale))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(timescaleImages[nextIndex].color.a < 0.9f) timescaleImages[nextIndex].color = Color.Lerp(timescaleImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
					else fadeIn = false;
				}
				else{
					if(timescaleImages[nextIndex].color.a > 0.1f) timescaleImages[nextIndex].color = Color.Lerp(timescaleImages[nextIndex].color, nextUpgradeColorTransparent, 0.07f);
					else fadeIn = true;
				}
			}
		}
	}
	
	void ClearTimescale(){
		currentIndex = 0;
		nextIndex = 0;
		
		for(int i = 0; i < 9; i++)
			timescaleImages[i].color = inactiveColor;
	}
	
	void IncreaseTimescale(){
		currentTimescale = currentTimescale + 1;
	}
}
