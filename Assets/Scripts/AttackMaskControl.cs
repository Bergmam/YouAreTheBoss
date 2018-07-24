using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMaskControl : MonoBehaviour
{

    Image mask;
    Image image;

    private float MIN_FAR_RADIUS = 14.5f;
    private float MAX_FAR_RADIUS = 80f;
    private float MIN_CLOSE_RADIUS = 95f;
    private float MAX_CLOSE_RADIUS = 530f;

    void Awake()
    {
        this.mask = GetComponent<Image>();
        this.image = transform.Find("Image").GetComponent<Image>();

        if (this.mask == null || this.image == null)
        {
            Destroy(this);
        }
    }

    public void SetSize(float closeRadiusScale, float farRadiusScale)
    {
        float closeRadius = MIN_CLOSE_RADIUS + closeRadiusScale * (MAX_CLOSE_RADIUS - MIN_CLOSE_RADIUS);
        float farRadius = MIN_FAR_RADIUS + farRadiusScale * (MAX_FAR_RADIUS - MIN_FAR_RADIUS);
        this.mask.rectTransform.sizeDelta = new Vector2(farRadius, farRadius);
        this.image.rectTransform.sizeDelta = new Vector2(closeRadius, closeRadius);
    }
}
