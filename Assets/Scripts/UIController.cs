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
}
