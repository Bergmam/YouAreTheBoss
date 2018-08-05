using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPopup : MonoBehaviour {

	GameObject infoPopUp;
	// Use this for initialization
	void Start () {
		infoPopUp = GameObject.Find("InfoPopUp");
		infoPopUp.SetActive(false);
	}
	
	// Update is called once per frame
	public void EnablePopUp(){
		if(infoPopUp.activeSelf == false) {
			infoPopUp.SetActive(true);
		} else {
			infoPopUp.SetActive(false);
		}
		
	}

	public void DisablePopUp(){
		infoPopUp.SetActive(false);
	}
}
