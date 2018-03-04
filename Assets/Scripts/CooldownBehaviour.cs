using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBehaviour : MonoBehaviour {

	private Image image;
	private float originalCooldownValue;
	private float cooldown;
	private Button parentButton;

	void Awake ()
	{
		this.parentButton = transform.parent.GetComponent<Button> ();
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
		else if(!this.parentButton.enabled)
		{
			this.parentButton.enabled = true;
		}
	}

	public void StartCooldown(float cooldown){
		this.parentButton.enabled = false;
		this.cooldown = cooldown;
		this.originalCooldownValue = cooldown;
	}

	public void RestartCooldown(){
		this.parentButton.enabled = false;
		this.cooldown = this.originalCooldownValue;
	}
}
