using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScaler : MonoBehaviour
{
    void Start()
    {
        Transform bossTransform = GameObject.Find("Boss").transform;
        bossTransform.localScale *= Parameters.SPRITE_SCALE_FACTOR;
    }
}
