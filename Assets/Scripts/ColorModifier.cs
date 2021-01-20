using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorModifier : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Image image;
    private Color selectedColor;
    private Color defaultColor;
    private float countDownTime;
    private float countDownStartTime;
    private bool fadeToSelected;
    private bool fadePaused;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {

        bool wasOverZero = countDownTime > 0;
        if (wasOverZero)
        {
            countDownTime -= Time.deltaTime;
            float proportion = countDownTime / countDownStartTime;
            if (!this.fadeToSelected)
            {
                proportion = 1 - proportion; //Change which color is according to proportion and which is the inverse
            }
            float newR = proportion * defaultColor.r + (1 - proportion) * selectedColor.r;
            float newB = proportion * defaultColor.b + (1 - proportion) * selectedColor.b;
            float newG = proportion * defaultColor.g + (1 - proportion) * selectedColor.g;
            float newA = proportion * defaultColor.a + (1 - proportion) * selectedColor.a;

            if (!this.fadePaused)
            {
                SetColor(new Color(newR, newG, newB, newA));
            }
        }
        bool reachedZero = countDownTime < 0 && wasOverZero; // Became less than zero after update.
        if (reachedZero)
        {
            SetSelected(this.fadeToSelected);
        }
    }

    public void FadeToSelected(float duration)
    {
        DeSelect();
        this.countDownStartTime = duration;
        this.countDownTime = duration;
        this.fadeToSelected = true;
    }

    public void FadeToDeselected(float duration)
    {
        Select();
        this.countDownStartTime = duration;
        this.countDownTime = duration;
        this.fadeToSelected = false;
    }

    public void setAlfa(float alfa)
    {
        this.SetColor(new Color(this.spriteRenderer.color.r, this.spriteRenderer.color.r, this.spriteRenderer.color.r, alfa));
    }

    public void SetSelectedColor(Color color)
    {
        this.selectedColor = color;
    }

    public void SetDefaultColor(Color color)
    {
        this.defaultColor = color;
        SetColor(defaultColor);
    }

    /// <summary>
    /// Changes color of the GameObject this script is attached to to its selectedColor.
    /// </summary>
    public void Select()
    {
        SetColor(selectedColor);
    }

    /// <summary>
    /// Changes color of the GameObject this script is attached to to its defaultColor.
    /// </summary>
    public void DeSelect()
    {
        SetColor(defaultColor);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            Select();
        }
        else
        {
            DeSelect();
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

    public Color GetColor()
    {
        if (this.spriteRenderer != null)
        {
            return this.spriteRenderer.color;
        }
        if (this.image != null)
        {
            return this.image.color;
        }
        return this.defaultColor;
    }

    public void StopFade()
    {
        this.countDownTime = 0;
    }

    public void SetFadePaused(bool fadePaused)
    {
        this.fadePaused = fadePaused;
        if (!fadePaused && this.countDownTime <= 0)
        {
            SetSelected(this.fadeToSelected);
        }
    }
}
