using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private GameObject pausePanel;
    public static bool gamePaused;

    void Awake() 
    {
        this.pausePanel = GameObject.Find("PausePanel");
        gamePaused = false;
    }

    void Start()
    {
        this.pausePanel.SetActive(false);
    }

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)){
            if (gamePaused)
            {
                this.UnpauseGame();
            }
            else
            {
                this.PauseGame();
            }
        }
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnpauseGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        this.pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        this.pausePanel.SetActive(true);
        //pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    } 
}