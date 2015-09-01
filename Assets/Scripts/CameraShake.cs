using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shake = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 1f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;

	Vector3 nextPos;

	float randomCount;

	float distance;



	void Awake()
	{

		randomCount = Random.Range(0.6f, 1f);
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
		//InvokeRepeating("GetDistance", randomCount, randomCount);
		StartCoroutine(CalculateDistance());
		Repeat();
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Repeat(){
		nextPos = originalPos + Random.insideUnitSphere * shakeAmount;
	}

	IEnumerator CalculateDistance(){
		while(true){
			distance = Vector3.Distance(camTransform.localPosition, nextPos);
			if(distance < (shakeAmount)){
				//distance = 100;
				Repeat();
			}
			randomCount = Random.Range(0.6f, 1.2f);
			yield return new WaitForSeconds(randomCount);
		}
	}

	void GetDistance(){
		distance = Vector3.Distance(camTransform.localPosition, nextPos);
		if(distance < (shakeAmount)){
			//distance = 100;
			Repeat();
		}
	}

	void Update()
	{

		print(distance + " / " +(shakeAmount));
		//randomCount = Random.Range(0.6f, 1f);
		camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, nextPos, Time.deltaTime * distance);
	}
}
