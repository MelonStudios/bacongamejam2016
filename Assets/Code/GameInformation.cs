using UnityEngine;
using System.Collections;

public class GameInformation : MonoBehaviour
{
    public static GameInformation Instance;

    public GameState GameState;
    public GameOverResult GameOverResult;

    private LevelInformation levelInformation;

    public LevelInformation LevelInformation
    {
        get
        {
            if (levelInformation == null)
            {
                levelInformation = FindObjectOfType<LevelInformation>();
            }

            return levelInformation;
        }
    }


    private PlayerInformation playerInformation;

    public PlayerInformation PlayerInformation
    {
        get
        {
            if (playerInformation == null)
            {
                playerInformation = FindObjectOfType<PlayerInformation>();
            }

            return playerInformation;
        }
    }

    void Awake()
    {
        Instance = this;
        GameState = GameState.PreGame;
    }

    void Update()
    {
        DebugController.Instance.LogLine("GAMESTATE: " + GameState);
    }
}

public enum GameState
{
    Menus,
    PreGame,
    Playing,
    Paused,
    PostGame,
    GameOver
}

public enum GameOverResult
{
    Win,
    Lose
}