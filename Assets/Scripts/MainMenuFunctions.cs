using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public void LoadGameLevel()
    {
        //SceneManager.LoadScene("MainScene");
        Debug.Log("Holy moly you're playing the game frfr ong");
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
    }
}
