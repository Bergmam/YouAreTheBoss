using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAttack : MonoBehaviour {


	float angle, closeRadius, farRadius;

	float damage;


	// Use this for initialization
	void Start () {
		angle = 30;
		closeRadius = 0;
		farRadius = 2.0f;
		damage = 50;
		InvokeRepeating("doAttack", 0, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void doAttack() {
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj){
			GameObject g = (GameObject) o;
			if (g.name.Contains("Enemy")){
				Enemy enemy = g.GetComponent<Enemy>();
				if (enemy.isInAttackArea(360 - transform.eulerAngles.z - angle, 360 - transform.eulerAngles.z + angle, closeRadius, farRadius)){
					enemy.applyDamageTo(damage);
				}
			}
		}

		// For now, change color of boss when he is attacking
	 	gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		
		StartCoroutine(attackColorChange(gameObject.GetComponent<SpriteRenderer>()));
		
	}

	IEnumerator attackColorChange(SpriteRenderer renderer)
    {
        yield return new WaitForSeconds(0.5f);
		renderer.color = Color.white;
    }
}
