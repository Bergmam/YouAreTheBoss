using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMaskControl : MonoBehaviour {

	Image mask;
	Image image;

	void Start ()
	{
		this.mask = GetComponent<Image> ();
		this.image = transform.FindChild ("Image").GetComponent<Image> ();

		if (this.mask == null || this.image == null)
		{
			Destroy (this);
		}
	}

	public void SetSize (float closeRadius, float farRadius)
	{
		farRadius *= 2;
		closeRadius *= 2;
		closeRadius = Mathf.Max (closeRadius, 1f);
		this.mask.rectTransform.sizeDelta = new Vector2 (farRadius, farRadius);
		this.image.rectTransform.sizeDelta = new Vector2 (closeRadius, closeRadius);
	}
}
