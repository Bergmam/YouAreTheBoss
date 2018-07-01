using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Camera mainCam;

	float shakeAmount = 0;

	void Awake () {
		if (mainCam == null) {
			mainCam = Camera.main;
		}
	}

	void Update(){
		if( Input.GetKeyDown(KeyCode.T)){
			Shake (0.1f, 0.2f);
		}
	}

	public void Shake (float amount, float length) {
		shakeAmount = amount;
		InvokeRepeating("DoShake", 0, 0.01f);
		Invoke("StopShake", length);
	}

	void DoShake () {
		if (shakeAmount > 0) {
			Vector3 camPos = mainCam.transform.position;

			float shakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
			float shakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;

			camPos.x += shakeAmtX;
			camPos.y += shakeAmtY;

			mainCam.transform.position = camPos;
		}
	}

	void StopShake () {
		CancelInvoke("DoShake");
		mainCam.transform.localPosition = new Vector3(0, -0.75f, -10);
	}
}
