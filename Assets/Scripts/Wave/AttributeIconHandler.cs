using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AttributeIconHandler : MonoBehaviour
{

    private Image icon;

    private int maxMagnitude = 20;
    private float minScale = 0.25f;
    private float maxScale = 1.0f;
    private Dictionary<string, string> attributeSpritePathDict = new Dictionary<string, string>()
    {
        {"durable", "Art/UI_Icon_Plus"}
    };

    void Start()
    {
        this.icon = gameObject.GetComponent<Image>();
        this.initialize("durable", 20);
    }

    public void initialize(string attribute, int magnitude)
    {
        magnitude = Mathf.Min(magnitude, maxMagnitude);
        magnitude = Mathf.Max(magnitude, 0);
        Sprite sprite = Resources.Load<Sprite>(attributeSpritePathDict[attribute]);
        this.icon.sprite = sprite;
        float scale = minScale + ((maxScale - minScale) * (float)magnitude / (float)maxMagnitude);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
