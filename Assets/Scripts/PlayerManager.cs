using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController[] players;
    int playersReadyCounter = 0;
    bool moveEnabled = true;

    // Start is called before the first frame update
    void Awake()
    {
        players = GetComponentsInChildren<PlayerController>();
        playersReadyCounter = players.Length;
    }

    void OnEnable()
    {
        EventManager.StartListening("ready", PlayerIsReady);
    }

    void OnDisable()
    {
        EventManager.StartListening("ready", PlayerIsReady);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2();
        input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            input.y = 0;
        } else
        {
            input.x = 0;
        }

        if (moveEnabled && input != Vector2.zero && playersReadyCounter == players.Length)
        {

            bool[] playerWillMove = new bool[players.Length];
            bool[] sliding = new bool[players.Length];
            for (int i = 0; i < players.Length; ++i)
            {
                playerWillMove[i] = true;
                sliding[i] = false;
            }
            Vector3Int[] playerNextTile = new Vector3Int[players.Length];
            Vector3[] playerDirection = new Vector3[players.Length];

            for (int i = 0; i < players.Length; ++i)
            {
                playerWillMove[i] = true;
                playerNextTile[i] = players[i].CalculateNextTile(input, out sliding[i]);
                playerDirection[i] = playerNextTile[i] - players[i].GetCurrentGridPosition();
                playerDirection[i] = playerDirection[i].normalized;
            }

            for (int i = 0; i < players.Length; ++i)
            {
                for(int j = 0; j < players.Length; ++j)
                {
                    if (i == j) continue;

                    if(playerDirection[i] != playerDirection[j])
                    {
                        if (playerNextTile[i] == players[j].GetCurrentGridPosition())
                        {
                            playerWillMove[i] = false;
                            playerWillMove[j] = false;
                        }
                    }

                    if (playerDirection[i] != Vector3Int.zero && playerDirection[i] == playerDirection[j] * -1)
                    {
                        if (playerNextTile[i] == playerNextTile[j])
                        {
                            playerWillMove[i] = false;
                            playerWillMove[j] = false;
                        }
                    }

                    if(playerNextTile[i] == playerNextTile[j] && sliding[i])
                    {
                        playerWillMove[i] = false;
                        playerWillMove[j] = true;
                    }
                }
            }

            for (int i = 0; i < players.Length; ++i)
            {
                playersReadyCounter--;
                players[i].move(playerNextTile[i], !playerWillMove[i], sliding[i]);
            }

            bool someoneIsSliding = false;
            for(int i = 0; i < players.Length; ++i)
            {
                if (sliding[i])
                {
                    someoneIsSliding = true;
                    break;
                }
            }
            if (someoneIsSliding)
            {
                AudioManager.Instance.PlaySlippery();
            }
            else
            {
                AudioManager.Instance.PlayMove();
            }
        }

        int[] stepsArray = new int[GetPlayers().Length];
        for (int i = 0; i < GetPlayers().Length; i++)
        {
            stepsArray[i] = GetPlayers()[i].steps;
        }
        foreach(PlayerController player in players)
        {
            player.steps = stepsArray.Max();
        }
    }

    public PlayerController[] GetPlayers()
    {
        return players;
    }

    public void SetMoveEnabled(bool enable)
    {
        moveEnabled = enable;
    }

    public bool IsEveryPlayerOnTarget()
    {
        foreach (PlayerController player in players)
        {
            if (!player.IsPlayerOverTarget())
                return false;
        }
        return true;
    }

    private void PlayerIsReady()
    {
        playersReadyCounter++;
    }
}
