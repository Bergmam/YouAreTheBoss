using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour {

	void Update ()
	{
		Vector3 touchPos = Vector3.zero;
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved){
				touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			}
		}
		else
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		if (touchPos.y > Parameters.ARENA_BOTTOM && !touchPos.Equals (Vector3.zero))
		{
			transform.rotation = Quaternion.LookRotation (Vector3.forward, touchPos - transform.position);
		}
	}
}
