using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public UIController uiController;
    public PlayerManager playerManager;

    [SerializeField]Sprite Bad, Medium, Good;

    private int s2 = 0; //1/3 more steps than our best
    [SerializeField] int s3 = 0; //our best

    [SerializeField] int steps = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiController.HideMenus();
        s2 = s3 *4/3;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            uiController.DisplayOrHidePause();
        }
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            ReloadLevel();
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("die", LevelFail);
        EventManager.StartListening("step", UpdateStepCounter);
        EventManager.StartListening("finish", CheckLevelCompleted);
    }


    void OnDisable()
    {
        EventManager.StopListening("die", LevelFail);
        EventManager.StopListening("step", UpdateStepCounter);
        EventManager.StopListening("finish", CheckLevelCompleted);
    }

    public void LoadMainMenu()
    {
        MenuController.LoadLevelsMenu();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void CheckLevelCompleted()
    {
        if(playerManager.IsEveryPlayerOnTarget())
        {
            LevelComplete();
        }
    }

    private void UpdateStepCounter()
    {
        Text stepsText = GameObject.Find("Steps").GetComponent<Text>();
        int[] stepsArray = new int[playerManager.GetPlayers().Length];
        for (int i = 0; i < playerManager.GetPlayers().Length; i++)
        {
            stepsArray[i] = playerManager.GetPlayers()[i].steps;
        }
        steps = stepsArray.Max();
        stepsText.text = "Steps: " + steps;
    }

    public void LevelComplete()
    {
        AudioManager.Instance.PlayWin();

        if (steps <= s3)
            uiController.SetCongratulations(Good);
        else if(steps <= s2)
            uiController.SetCongratulations(Medium);
        else
            uiController.SetCongratulations(Bad);
            
        playerManager.SetMoveEnabled(false);
        uiController.DisplayCongratulations();
    }

    public void LevelFail()
    {
        AudioManager.Instance.PlayLost();

        playerManager.SetMoveEnabled(false);
        uiController.DisplayFail();
    }
}
