using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeController : MonoBehaviour
{
    private static float SPAWN_RADIUS_PADDING = 2.2f;

    void Start()
    {
        // Make sure the screen is wide enough so attacks don't reach off screen.
        float aspectRatio = ((float)Screen.height / (float)Screen.width);
        Camera.main.orthographicSize = (Parameters.ENEMY_SPAWN_RADIUS - SPAWN_RADIUS_PADDING) * aspectRatio;
    }
}