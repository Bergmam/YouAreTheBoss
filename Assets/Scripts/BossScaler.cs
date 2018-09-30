using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScaler : MonoBehaviour
{
    void Start()
    {
        Transform bossTransform = GameObject.Find("Boss").transform;
        Transform bossCanvasTransform = bossTransform.Find("Canvas");
        bossTransform.localScale *= Parameters.SPRITE_SCALE_FACTOR;
        bossCanvasTransform.localScale *= 1.0f / Parameters.SPRITE_SCALE_FACTOR;
    }
}
