using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialFillControl : MonoBehaviour {

	Image image;

	void Start ()
	{
		this.image = GetComponent<Image> ();
		if (this.image == null) {
			Destroy (this);
		}

		//Make sure the image the script is attached to is actually filled radially.
		this.image.type = Image.Type.Filled;
		this.image.fillMethod = Image.FillMethod.Radial360;
	}

	/// <summary>
	/// Sets the fill to the specified number of degrees on each side of the center.
	/// </summary>
	/// <param name="degrees">Degrees.</param>
	public void SetMirroredFill (int degrees)
	{
		this.image.fillAmount = (float)(degrees * 2) / 360; // Expand radial fill to twice the fill ammount.

		//Rotate the image so that the center of the fill is in the same direction.
		float currentRotX = this.transform.localEulerAngles.x;
		float currentRotY = this.transform.localEulerAngles.y;
		this.transform.localEulerAngles = new Vector3 (currentRotX, currentRotY, degrees);
	}


	// USED FOR DEBUGGING PURPOSES ONLY
	// Expands image automatically over time.
	/*
	int delay;
	int degrees;
	void Update ()
	{
		if (delay <= 0) {
			degrees += 5;
			SetMirroredFill (degrees);
			delay = 30;
		} else {
			delay--;
		}
	}
	*/
}
