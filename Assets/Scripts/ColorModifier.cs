using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorModifier : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Image image;
	private Color selectedColor = new Color32 (135, 0, 0, 255);
	private Color defaultColor = Color.white;
	private float countDownTime;
	private float countDownStartTime;
	private bool fadeToSelected;

	void Awake ()
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		image = gameObject.GetComponent<Image> ();
	}

	void Update()
	{
		bool wasOverZero = countDownTime > 0;
		if(wasOverZero){
			countDownTime-=Time.deltaTime;
			float proportion = countDownTime / countDownStartTime;
			if(!fadeToSelected){
				proportion = 1 - proportion; //Change which color is according to proportion and which is the inverse
			}
			float newR = proportion * defaultColor.r + (1 - proportion) * selectedColor.r;
			float newB = proportion * defaultColor.b + (1 - proportion) * selectedColor.b;
			float newG = proportion * defaultColor.g + (1 - proportion) * selectedColor.g;
			float newA = proportion * defaultColor.a + (1 - proportion) * selectedColor.a;
			SetColor(new Color(newA,newB,newG,newA));
		}
		bool reachedZero = countDownTime < 0 && wasOverZero; // Became less than zero after update.
		if(reachedZero){
			if(fadeToSelected){
				Select();
			} else {
				DeSelect();
			}
		}
	}

	public void FadeToSelected(float duration)
	{
		DeSelect();
		this.countDownStartTime = duration;
		this.countDownTime = duration;
		this.fadeToSelected = true;
	}

	public void FadeToDelected(float duration)
	{
		Select();
		this.countDownStartTime = duration;
		this.countDownTime = duration;
		this.fadeToSelected = false;
	}

	public void SetSelectedColor(Color color)
	{
		this.selectedColor = color;
	}

	public void SetDefaultColor(Color color)
	{
		this.defaultColor = color;
		SetColor (defaultColor);
	}

	/// <summary>
	/// Changes color of the GameObject this script is attached to to its selectedColor.
	/// </summary>
	public void Select()
	{
		SetColor (selectedColor);
	}

	/// <summary>
	/// Changes color of the GameObject this script is attached to to its defaultColor.
	/// </summary>
	public void DeSelect()
	{
		SetColor (defaultColor);
	}

	public void SetSelected(bool selected)
	{
		if (selected)
		{
			Select ();
		}
		else
		{
			DeSelect ();
		}
	}

	/// <summary>
	/// Sets the color of the GameObject this script is attached to.
	/// </summary>
	/// <param name="color">Color.</param>
	public void SetColor(Color color)
	{
		if (this.spriteRenderer != null)
		{
			this.spriteRenderer.color = color;
		}
		if (this.image != null)
		{
			this.image.color = color;
		}
	}
}
