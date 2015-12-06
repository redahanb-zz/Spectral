//Name:			DisableOnAwake.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Basic script used to disable a gameobject on Awake


using UnityEngine;
using System.Collections;

public class DisableOnAwake : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.SetActive(false);
	}
}
