using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour {

	void Update () {
		if (Input.touchCount > 0){
			if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved){
				Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				transform.rotation = Quaternion.LookRotation(Vector3.forward, touchPos - transform.position);
			}
		} else {
			Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.rotation = Quaternion.LookRotation(Vector3.forward, touchPos - transform.position);
		}
		
	}
}
