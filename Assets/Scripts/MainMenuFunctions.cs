using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public void LoadGameLevel1()
    {
        SceneManager.LoadScene("Level1");
        Debug.Log("Holy moly you're playing the game frfr ong");
    }

    public void LoadGameLevel2()
    {
        SceneManager.LoadScene("Level2");
        Debug.Log("Holy moly you're playing the game 2 frfr ong");
    }

    public void LoadGameLevel3()
    {
        SceneManager.LoadScene("Level3");
        Debug.Log("Holy moly you're playing the game 3 frfr ong");
    }

    public void LoadGameLevel4()
    {
        SceneManager.LoadScene("Level 4");
        Debug.Log("Holy moly you're playing the game 4 frfr ong");
    }

    public void LoadGameTest()
    {
        SceneManager.LoadScene("TestingGround");
        Debug.Log("Holy moly you're not playing the game cause you wanna 'test'? lame");
    }

    public void DisableScreen(GameObject screen)
    {
        screen.SetActive(false);
    }

    public void EnableScreen(GameObject screen)
    {
        screen.SetActive(true);
    }
    public void QuitButton()
    {
        Debug.Log("Quit The Game");
        Application.Quit();
    }
}
