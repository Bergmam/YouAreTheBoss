using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotater : MonoBehaviour {

	GameObject rotator1;
	GameObject rotator2;
	GameObject rotator3;

	List<GameObject> rotators = new List<GameObject>();
	Dictionary<string, float> rotatorSpeeds = new Dictionary<string, float>();
	
	void Start () {
		rotator1 = GameObject.Find("Rotator 1");
		rotators.Add(rotator1);
		rotator2 = GameObject.Find("Rotator 2");
		rotators.Add(rotator2);
		rotator3 = GameObject.Find("Rotator 3");
		rotators.Add(rotator3);

		foreach(GameObject rotator in rotators){
			rotatorSpeeds[rotator.name] = UnityEngine.Random.Range(-0.4f, 0.4f);
		}
	}

	void Update() {
		foreach(GameObject rotator in rotators){
			rotator.transform.Rotate(new Vector3(0, 0, rotatorSpeeds[rotator.name]));
		}
	}
}
