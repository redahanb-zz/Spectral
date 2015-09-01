using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeMoveSpeed : MonoBehaviour {
	
	public 	bool 	speedVisible = false;
	public 	int 	currentSpeed = 5;
	
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
	
	private 		RawImage[] speedImages;
	private 		RectTransform speedInfoTransform;
	
	
	Image			speedButtonImage;
	
	Color		buttonTargetColor = Color.white;
	// Use this for initialization
	void Start () {
		
		speedButtonImage = GameObject.Find("Move Speed Button").GetComponent<Image>();
		buttonTargetColor = Color.white;
		speedInfoTransform = transform.GetComponent<RectTransform>();
		
		hiddenPos = speedInfoTransform.position;
		visiblePos = speedInfoTransform.position + new Vector3(340,0,0);
		speedInfoTransform.position = hiddenPos;
		
		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);
		
		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);
		
		speedImages = new RawImage[9];
		speedImages[0] = transform.Find("Current Speed").Find("Speed Bar").Find("1").GetComponent<RawImage>();
		speedImages[1] = transform.Find("Current Speed").Find("Speed Bar").Find("2").GetComponent<RawImage>();
		speedImages[2] = transform.Find("Current Speed").Find("Speed Bar").Find("3").GetComponent<RawImage>();
		speedImages[3] = transform.Find("Current Speed").Find("Speed Bar").Find("4").GetComponent<RawImage>();
		speedImages[4] = transform.Find("Current Speed").Find("Speed Bar").Find("5").GetComponent<RawImage>();
		speedImages[5] = transform.Find("Current Speed").Find("Speed Bar").Find("6").GetComponent<RawImage>();
		speedImages[6] = transform.Find("Current Speed").Find("Speed Bar").Find("7").GetComponent<RawImage>();
		speedImages[7] = transform.Find("Current Speed").Find("Speed Bar").Find("8").GetComponent<RawImage>();
		speedImages[8] = transform.Find("Current Speed").Find("Speed Bar").Find("9").GetComponent<RawImage>();
		
		ClearSpeed();
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		MovePanel();
		if(Vector3.Distance(speedInfoTransform.position,visiblePos) < 5){
			FillSpeed();
			speedVisible = true;
		}
		if(Vector3.Distance(speedInfoTransform.position,visiblePos) > 330){
			ClearSpeed();
			speedVisible = false;
		}
		
		//Debug Input
		//if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
		//if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;
		
	}
	
	void MovePanel(){
		if(displayPanel){
			speedInfoTransform.position = Vector3.Lerp(speedInfoTransform.position, visiblePos, 0.1f);
			buttonTargetColor = Color.green;
		}
		else{
			speedInfoTransform.position = Vector3.Lerp(speedInfoTransform.position, hiddenPos, 0.1f);
			buttonTargetColor = Color.white;
		}
		speedButtonImage.color = Color.Lerp(speedButtonImage.color,buttonTargetColor, Time.deltaTime * 5);
	}
	
	void FillSpeed(){
		if(speedImages[currentIndex].color.a < 0.95f){
			speedImages[currentIndex].color = Color.Lerp(speedImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (currentSpeed))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(speedImages[nextIndex].color.a < 0.9f) speedImages[nextIndex].color = Color.Lerp(speedImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
					else fadeIn = false;
				}
				else{
					if(speedImages[nextIndex].color.a > 0.1f) speedImages[nextIndex].color = Color.Lerp(speedImages[nextIndex].color, nextUpgradeColorTransparent, 0.07f);
					else fadeIn = true;
				}
			}
		}
	}
	
	void ClearSpeed(){
		currentIndex = 0;
		nextIndex = 0;
		
		for(int i = 0; i < 9; i++)
			speedImages[i].color = inactiveColor;
	}
	
	void IncreaseSpeed(){
		currentSpeed = currentSpeed + 1;
	}
}
