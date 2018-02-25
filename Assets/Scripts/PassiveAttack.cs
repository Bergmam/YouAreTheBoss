using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAttack : MonoBehaviour {


	float angle, closeRadius, farRadius;


	// Use this for initialization
	void Start () {
		angle = 22.5f;
		closeRadius = 0;
		farRadius = 2.0f;

	}
	
	// Update is called once per frame
	void Update () {
		doAttack();
	}

	void doAttack() {
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj){
			GameObject g = (GameObject) o;
			if (g.name.Contains("Enemy")){
				Debug.Log("Boss rotation.z:" + transform.eulerAngles.z);
				if (g.GetComponent<Enemy>().isInAttackArea(360 - transform.eulerAngles.z - angle, 360 - transform.eulerAngles.z + angle, closeRadius, farRadius)){
					Destroy(g);
				}
			}
		}
	}
}
