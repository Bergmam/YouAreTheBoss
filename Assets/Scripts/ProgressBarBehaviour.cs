using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for handling the behaviour of the progress bar in the MoveEditor.
/// </summary>
public class ProgressBarBehaviour : MonoBehaviour 
{
	private RectTransform fill;
	private Vector2 twoByTwoVector;
	private Vector2 oneByOneVector;
	private int direction;

	void Awake()
	{
		this.fill = transform.Find ("Fill").gameObject.GetComponent<RectTransform>();
		this.twoByTwoVector = new Vector2 (2, 2);
		this.oneByOneVector = new Vector2 (1, 1);
		this.direction = 1;
	}

	/// <summary>
	/// Updates the fill to show the number of recorded frames.
	/// </summary>
	public void UpdateFill(float progress)
	{
		//Make sure fill never extends outside of container.
		if (progress < 0)
		{
			progress = 0;
		}
		else if (progress > 1)
		{
			progress = 1;
		}
		if (direction >= 0)
		{
			//Bounds = bot = 0, top = 1, left = 0, right = percentage of total frames recorded
			fill.anchorMin = Vector2.zero;
			fill.anchorMax = new Vector2 (progress, 1);
		}
		else
		{
			//Bounds = bot = 0, top = 1, left = percentage of total frames recorded, right = 1
			fill.anchorMin = new Vector2 (1 - progress, 0);
			fill.anchorMax = oneByOneVector;
		}
		//Create a border of unit length 2 around fill
		fill.offsetMax = twoByTwoVector;
		fill.offsetMin = twoByTwoVector;
		//Make sure fill is placed in middle of anchor
		fill.anchoredPosition = Vector2.zero;
	}

	/// <summary>
	/// Sets the direction. A positive direction means the bar extends to the right, a negative means it extends to the left.
	/// </summary>
	/// <param name="newDirection">New direction.</param>
	public void SetDirection(int newDirection)
	{
		this.direction = newDirection;
	}
}
