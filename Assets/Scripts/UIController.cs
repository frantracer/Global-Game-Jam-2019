using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void DisplayPause()
    {
        pauseMenu.SetActive(true);
        congratulationsMenu.SetActive(false);
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
}
