using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonsImageLoader : MonoBehaviour
{
    public int number;

    void Start()
    {
        PassiveAttack passiveAttack = Transform.FindObjectOfType<PassiveAttack>();
        BossAttack attack = passiveAttack.GetAttack(number);
        Transform imageTransform = transform.Find("Image");
        if (imageTransform != null)
        {
            Image image = imageTransform.GetComponent<Image>();
            image.sprite = image.sprite = Resources.Load<Sprite>(AttackLists.GetAssetString(attack.name));
        }
    }
}
