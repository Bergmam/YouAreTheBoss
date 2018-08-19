using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundRotater : MonoBehaviour {

	GameObject rotator1;
	GameObject rotator2;
	GameObject rotator3;
	GameObject rotator4;

	List<GameObject> rotators = new List<GameObject>();
	Dictionary<string, float> rotatorSpeeds = new Dictionary<string, float>();
	
	void Start () {
		rotator1 = GameObject.Find("Rotator 1");
		rotators.Add(rotator1);
		rotator2 = GameObject.Find("Rotator 2");
		rotators.Add(rotator2);
		rotator3 = GameObject.Find("Rotator 3");
		rotators.Add(rotator3);

		Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f, 0.6f, 0.6f);

		foreach(GameObject rotator in rotators){
			rotatorSpeeds[rotator.name] = UnityEngine.Random.Range(-0.4f, 0.4f);
			rotator.GetComponent<Image>().color = randomColor;
			while(rotatorSpeeds[rotator.name] == 0.0f){
				rotatorSpeeds[rotator.name] = UnityEngine.Random.Range(-0.4f, 0.4f);
			}
		}
	}

	void Update() {
		foreach(GameObject rotator in rotators){
			rotator.transform.Rotate(new Vector3(0, 0, rotatorSpeeds[rotator.name]));
		}
	}
}
