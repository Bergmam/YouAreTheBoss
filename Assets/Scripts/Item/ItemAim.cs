using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAim : MonoBehaviour
{
    public void Activate(float seconds)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(UnityUtils.DeactiveGameObjectAfterTime(this.gameObject, seconds));
    }
}
