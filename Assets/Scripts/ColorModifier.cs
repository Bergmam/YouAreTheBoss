using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Color modifier. Used to change the color of a GameObject between predefined colors.
/// </summary>
public class ColorModifier : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private Image image;
	private Color selectedColor = new Color32 (135, 0, 0, 255);
	private Color defaultColor = Color.white;

	void Awake ()
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		image = gameObject.GetComponent<Image> ();
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
