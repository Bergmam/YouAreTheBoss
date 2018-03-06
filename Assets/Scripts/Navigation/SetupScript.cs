using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handling the instantiation of the SceneHandler.
/// </summary>
public class SetupScript : MonoBehaviour 
{
	private static bool alreadyInitialized = false;

	void Start () 
	{
		if (!alreadyInitialized)
		{
			SceneHandler.Init (); //Instantiate SceneHandler with list of existing scenes and return stack
		}
	}
}
