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

    public void SetSize(float attackCloseRadius, float attackFarRadius)
    {
        float closeRadiusScale = (attackCloseRadius - Parameters.MIN_ATTACK_RADIUS) / (Parameters.MAX_ATTACK_RADIUS - Parameters.MIN_ATTACK_RADIUS);
        float farRadiusScale = (attackFarRadius - Parameters.MIN_ATTACK_RADIUS) / (Parameters.MAX_ATTACK_RADIUS - Parameters.MIN_ATTACK_RADIUS);
        float maskCloseRadius = MIN_CLOSE_RADIUS + closeRadiusScale * (MAX_CLOSE_RADIUS - MIN_CLOSE_RADIUS);
        float maskFarRadius = MIN_FAR_RADIUS + farRadiusScale * (MAX_FAR_RADIUS - MIN_FAR_RADIUS);
        this.mask.rectTransform.sizeDelta = new Vector2(maskFarRadius, maskFarRadius);
        this.image.rectTransform.sizeDelta = new Vector2(maskCloseRadius, maskCloseRadius);
    }
}
