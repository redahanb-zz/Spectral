using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeMoveNoise : MonoBehaviour {
	
	public 	bool 	noiseVisible = false;
	public 	int 	currentNoise = 5;
	
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
	
	private 		RawImage[] noiseImages;
	private 		RectTransform noiseInfoTransform;
	
	
	Image			noiseButtonImage;
	
	Color		buttonTargetColor = Color.white;
	// Use this for initialization
	void Start () {
		
		noiseButtonImage = GameObject.Find("Move Noise Button").GetComponent<Image>();
		buttonTargetColor = Color.white;
		noiseInfoTransform = transform.GetComponent<RectTransform>();
		
		hiddenPos = noiseInfoTransform.position;
		visiblePos = noiseInfoTransform.position + new Vector3(340,0,0);
		noiseInfoTransform.position = hiddenPos;
		
		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);
		
		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);
		
		noiseImages = new RawImage[9];
		noiseImages[0] = transform.Find("Current Noise").Find("Noise Bar").Find("1").GetComponent<RawImage>();
		noiseImages[1] = transform.Find("Current Noise").Find("Noise Bar").Find("2").GetComponent<RawImage>();
		noiseImages[2] = transform.Find("Current Noise").Find("Noise Bar").Find("3").GetComponent<RawImage>();
		noiseImages[3] = transform.Find("Current Noise").Find("Noise Bar").Find("4").GetComponent<RawImage>();
		noiseImages[4] = transform.Find("Current Noise").Find("Noise Bar").Find("5").GetComponent<RawImage>();
		noiseImages[5] = transform.Find("Current Noise").Find("Noise Bar").Find("6").GetComponent<RawImage>();
		noiseImages[6] = transform.Find("Current Noise").Find("Noise Bar").Find("7").GetComponent<RawImage>();
		noiseImages[7] = transform.Find("Current Noise").Find("Noise Bar").Find("8").GetComponent<RawImage>();
		noiseImages[8] = transform.Find("Current Noise").Find("Noise Bar").Find("9").GetComponent<RawImage>();
		
		ClearNoise();
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		MovePanel();
		if(Vector3.Distance(noiseInfoTransform.position,visiblePos) < 5){
			FillNoise();
			noiseVisible = true;
		}
		if(Vector3.Distance(noiseInfoTransform.position,visiblePos) > 330){
			ClearNoise();
			noiseVisible = false;
		}
		
		//Debug Input
		//if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
		//if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;
		
	}
	
	void MovePanel(){
		if(displayPanel){
			noiseInfoTransform.position = Vector3.Lerp(noiseInfoTransform.position, visiblePos, 0.1f);
			buttonTargetColor = Color.green;
		}
		else{
			noiseInfoTransform.position = Vector3.Lerp(noiseInfoTransform.position, hiddenPos, 0.1f);
			buttonTargetColor = Color.white;
		}
		noiseButtonImage.color = Color.Lerp(noiseButtonImage.color,buttonTargetColor, Time.deltaTime * 5);
	}
	
	void FillNoise(){
		if(noiseImages[currentIndex].color.a < 0.95f){
			noiseImages[currentIndex].color = Color.Lerp(noiseImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (currentNoise))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(noiseImages[nextIndex].color.a < 0.9f) noiseImages[nextIndex].color = Color.Lerp(noiseImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
					else fadeIn = false;
				}
				else{
					if(noiseImages[nextIndex].color.a > 0.1f) noiseImages[nextIndex].color = Color.Lerp(noiseImages[nextIndex].color, nextUpgradeColorTransparent, 0.07f);
					else fadeIn = true;
				}
			}
		}
	}
	
	void ClearNoise(){
		currentIndex = 0;
		nextIndex = 0;
		
		for(int i = 0; i < 9; i++)
			noiseImages[i].color = inactiveColor;
	}
	
	void IncreaseNoise(){
		currentNoise = currentNoise + 1;
	}
}
