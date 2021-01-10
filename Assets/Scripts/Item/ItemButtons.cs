using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtons : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject itemButtonGameObject = this.transform.GetChild(i).gameObject;
            itemButtonGameObject.GetComponent<ItemButton>().Index = i;
            if (BossItemHolder.BossItems[i] == null)
            {
                itemButtonGameObject.SetActive(false);
            }
        }
    }

    public void AddItem(ItemData item)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject itemButtonGameObject = this.transform.GetChild(i).gameObject;
            if (!itemButtonGameObject.activeSelf)
            {
                BossItemHolder.BossItems[i] = item;
                itemButtonGameObject.SetActive(true);
                return;
            }
        }
    }
}
