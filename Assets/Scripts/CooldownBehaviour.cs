using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBehaviour : MonoBehaviour {

	private Image image;
	private float originalCooldownValue;
	private float cooldown;

	void Awake ()
	{
		this.image = GetComponent<Image> ();
		if (this.image == null) {
			Destroy (this);
		}

		//Make sure the image the script is attached to is actually filled radially.
		this.image.type = Image.Type.Filled;
		this.image.fillMethod = Image.FillMethod.Radial360;
	}

	void Update () {
		if (this.cooldown > 0)
		{
			this.cooldown -= Time.deltaTime;
			this.image.fillAmount = cooldown / originalCooldownValue;
		}
	}

	public void StartCooldown(float cooldown){
		this.cooldown = cooldown;
		this.originalCooldownValue = cooldown;
	}

	public void RestartCooldown(){
		this.cooldown = this.originalCooldownValue;
	}
}
