using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtons : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform itemButtonTransform = this.transform.GetChild(i);
            GameObject itemButtonGameObject = itemButtonTransform.gameObject;
            itemButtonGameObject.GetComponent<ItemButton>().Index = i;
            ItemData item = BossItemHolder.BossItems[i];
            if (item == null)
            {
                itemButtonGameObject.SetActive(false);
            }
            else
            {
                Transform imageTransform = itemButtonTransform.Find("Image");
                Image image = imageTransform.GetComponent<Image>();
                image.sprite = item.Sprite;
            }
        }
    }

    public void AddItem(ItemData item)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform itemButtonTransform = this.transform.GetChild(i);
            GameObject itemButtonGameObject = itemButtonTransform.gameObject;
            if (!itemButtonGameObject.activeSelf)
            {
                BossItemHolder.BossItems[i] = item;

                Transform imageTransform = itemButtonTransform.Find("Image");
                Image image = imageTransform.GetComponent<Image>();
                image.sprite = item.Sprite;

                itemButtonGameObject.SetActive(true);
                return;
            }
        }
    }
}
