using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject congratulationsMenu;
    public GameObject tryagainMenu;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GameObject.Find("Characters").GetComponent<PlayerManager>();
    }

    public void DisplayCongratulations()
    {
        pauseMenu.SetActive(false);
        congratulationsMenu.SetActive(true);
    }

    public void DisplayFail()
    {
        pauseMenu.SetActive(false);
        tryagainMenu.SetActive(true);
    }

    public void DisplayOrHidePause()
    {
        if(pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        } else
        {
            pauseMenu.SetActive(true);
        }
    }

    public void HideMenus()
    {
        pauseMenu.SetActive(false);
        congratulationsMenu.SetActive(false);
    }

    internal void SetCongratulations(Sprite face)
    {
        congratulationsMenu.GetComponent<Image>().sprite = face;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        MenuController.LoadLevelsMenu();
    }

    public void SendInput(string direction)
    {
        Vector2 input = Vector2.zero;
        switch(direction)
        {
            case "up":
                input = Vector2.up;
                break;
            case "down":
                input = Vector2.down;
                break;
            case "left":
                input = Vector2.left;
                break;
            case "right":
                input = Vector2.right;
                break;
        }
        playerManager.SetInput(input);
    }
}
