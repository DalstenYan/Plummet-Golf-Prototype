using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu pauseMenu;
    //Setting variables
    public bool isPause = false;

    /// <summary>
    /// Automatically disable the pause menu on start
    /// </summary>
    void Awake()
    {
        Time.timeScale = 1;
        pauseMenu = this;
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Reads the "Pause" input and stops the game's processing until unpaused
    /// </summary>
    public void OnPause()
    {
       GameObject.FindWithTag("Player").GetComponent<BallController>().TogglePauseControls();
        PauseActivate();
    }

    /// <summary>
    /// Reloads the current level when the pause menu "Restart" button is pressed
    /// </summary>
    public void Restart()
    {
        PauseActivate(); //Unpauses the game for the reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Gets the name of the currently active scene
    }

    /// <summary>
    /// Return to the main menu when the pause menu "Main Menu" button is pressed
    /// </summary>
    public void MainMenu()
    {
        PauseActivate(); //Unpauses the game before returning
        SceneManager.LoadScene("MainMenu");
    }

    public void OneShotMode() 
    {
        UIManager.Instance.ToggleOneShotMode();
        GameObject.FindGameObjectWithTag("Player").GetComponent<BallController>().ToggleOneShotMode();
        OnPause();
    }

    /// <summary>
    /// Pause function to be easily reused in multiple places
    /// </summary>
    private void PauseActivate()
    {
        if (!isPause)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //  PauseBG.enabled = true;
            gameObject.SetActive(true);
            isPause = true;

        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //    PauseBG.enabled = false;
            gameObject.SetActive(false);
            isPause = false;
        }

    }
}
