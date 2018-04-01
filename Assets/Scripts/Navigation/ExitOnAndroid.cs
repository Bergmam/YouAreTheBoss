using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handling the instantiation of the SceneHandler.
/// </summary>
public class ExitOnAndroid : MonoBehaviour 
{
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
	}
}
