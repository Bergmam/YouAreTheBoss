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

		if (BossHealthHolder.BossHealth <= 0)
		{
			BossHealthHolder.BossHealth = BossHealthHolder.BossFullHealth;
		}
		
        ProgressBarBehaviour bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<ProgressBarBehaviour>();
        bossHealthBar.UpdateFill(BossHealthHolder.BossHealth / BossHealthHolder.BossFullHealth);
	}
}
