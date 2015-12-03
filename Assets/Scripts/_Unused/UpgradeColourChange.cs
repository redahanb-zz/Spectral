using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeColourChange : MonoBehaviour {
	
	public 	bool 	colourVisible = false;
	public 	int 	currentColour = 5;
	
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
	
	private 		RawImage[] colourImages;
	private 		RectTransform colourInfoTransform;
	
	
	Image			colourButtonImage;
	
	Color		buttonTargetColor = Color.white;
	// Use this for initialization
	void Start () {
		
		colourButtonImage = GameObject.Find("Colour Speed Button").GetComponent<Image>();
		buttonTargetColor = Color.white;
		colourInfoTransform = transform.GetComponent<RectTransform>();
		
		hiddenPos = colourInfoTransform.position;
		visiblePos = colourInfoTransform.position + new Vector3(340,0,0);
		colourInfoTransform.position = hiddenPos;
		
		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);
		
		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);
		
		colourImages = new RawImage[9];
		colourImages[0] = transform.Find("Current Colour").Find("Colour Bar").Find("1").GetComponent<RawImage>();
		colourImages[1] = transform.Find("Current Colour").Find("Colour Bar").Find("2").GetComponent<RawImage>();
		colourImages[2] = transform.Find("Current Colour").Find("Colour Bar").Find("3").GetComponent<RawImage>();
		colourImages[3] = transform.Find("Current Colour").Find("Colour Bar").Find("4").GetComponent<RawImage>();
		colourImages[4] = transform.Find("Current Colour").Find("Colour Bar").Find("5").GetComponent<RawImage>();
		colourImages[5] = transform.Find("Current Colour").Find("Colour Bar").Find("6").GetComponent<RawImage>();
		colourImages[6] = transform.Find("Current Colour").Find("Colour Bar").Find("7").GetComponent<RawImage>();
		colourImages[7] = transform.Find("Current Colour").Find("Colour Bar").Find("8").GetComponent<RawImage>();
		colourImages[8] = transform.Find("Current Colour").Find("Colour Bar").Find("9").GetComponent<RawImage>();
		
		ClearColour();
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		MovePanel();
		if(Vector3.Distance(colourInfoTransform.position,visiblePos) < 5){
			FillColour();
			colourVisible = true;
		}
		if(Vector3.Distance(colourInfoTransform.position,visiblePos) > 330){
			ClearColour();
			colourVisible = false;
		}
		
		//Debug Input
		//if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
		//if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;
		
	}
	
	void MovePanel(){
		if(displayPanel){
			colourInfoTransform.position = Vector3.Lerp(colourInfoTransform.position, visiblePos, 0.1f);
			buttonTargetColor = Color.green;
		}
		else{
			colourInfoTransform.position = Vector3.Lerp(colourInfoTransform.position, hiddenPos, 0.1f);
			buttonTargetColor = Color.white;
		}
		colourButtonImage.color = Color.Lerp(colourButtonImage.color,buttonTargetColor, Time.deltaTime * 5);
	}
	
	void FillColour(){
		if(colourImages[currentIndex].color.a < 0.95f){
			colourImages[currentIndex].color = Color.Lerp(colourImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (currentColour))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(colourImages[nextIndex].color.a < 0.9f) colourImages[nextIndex].color = Color.Lerp(colourImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
					else fadeIn = false;
				}
				else{
					if(colourImages[nextIndex].color.a > 0.1f) colourImages[nextIndex].color = Color.Lerp(colourImages[nextIndex].color, nextUpgradeColorTransparent, 0.07f);
					else fadeIn = true;
				}
			}
		}
	}
	
	void ClearColour(){
		currentIndex = 0;
		nextIndex = 0;
		
		for(int i = 0; i < 9; i++)
			colourImages[i].color = inactiveColor;
	}
	
	void IncreaseColour(){
		currentColour = currentColour + 1;
	}
}
