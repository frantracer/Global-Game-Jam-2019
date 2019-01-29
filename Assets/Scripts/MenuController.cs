using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private static string menuToLoad = "main";

    public static string sceneMenuSceneName = "MainMenuScene";

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void LoadLevel(int levelid)
    {
        SceneManager.LoadScene(levelid, LoadSceneMode.Single);
    }

    public static void LoadLevelsMenu()
    {
        SceneManager.LoadScene(sceneMenuSceneName, LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
