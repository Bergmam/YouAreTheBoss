using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AttributeIconHandler : MonoBehaviour
{

    private Image icon;

    // TODO: Adjust these values
    private int maxMagnitude = 20;
    private float minScale = 0.3f;
    private float maxScale = 1.0f;

    private Dictionary<string, string> attributeSpritePathDict = new Dictionary<string, string>()
    {
        {"strong", "Art/UI_Icon_Skull"},
        {"fast", "Art/UI_Icon_FastForward"},
        {"rotating", "Art/UI_Icon_Reload"},
        {"ranged", "Art/UI_Icon_Archer"},
        {"durable", "Art/UI_Icon_Plus"},
        {"mele", "Art/UI_Icon_Attack"},
        {"self_destruct", "Art/UI_Icon_Bomb"},
        {"unknown", "Art/UI_Icon_Question"}
    };

    public void SetAttributeAndMagnitude(string attribute, int magnitude)
    {
        Transform imageTransform = transform.Find("Image");
        this.icon = imageTransform.GetComponent<Image>();
        magnitude = Mathf.Min(magnitude, maxMagnitude);
        magnitude = Mathf.Max(magnitude, 0);
        Sprite sprite;
        if(attributeSpritePathDict.ContainsKey(attribute))
        {
            sprite = Resources.Load<Sprite>(attributeSpritePathDict[attribute]);
        } else {
            sprite = Resources.Load<Sprite>(attributeSpritePathDict["unknown"]);
        }
        this.icon.sprite = sprite;

        // Determine the percentage of parent panel an icon should fill based on the minimum and maximum amount they are allowed to fill.
        float scale = minScale + ((maxScale - minScale) * (float)magnitude / (float)maxMagnitude);
        float anchorOffset = (1.0f - scale) / 2.0f;

        // Make icon fill the decided percantage of parent.
        RectTransform imageRectTransform = imageTransform.GetComponent<RectTransform>();
        imageRectTransform.anchorMax = new Vector2(1.0f - anchorOffset, 1.0f - anchorOffset);
        imageRectTransform.anchorMin = new Vector2(anchorOffset, anchorOffset);
        imageRectTransform.offsetMax = Vector2.zero;
        imageRectTransform.offsetMin = Vector2.zero;
    }
}
