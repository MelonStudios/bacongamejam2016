using UnityEngine;
using System.Collections;

public class PlayingController : MonoBehaviour
{
    public Canvas DeathCanvas;
    
    void Awake()
    {
        DeathCanvas.enabled = false;
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;

        GameInformation.Instance.LevelInformation.TimeRemaining -= Time.deltaTime;

        if (GameInformation.Instance.PlayerInformation.CharacterState == CharacterState.Dead ||
            GameInformation.Instance.LevelInformation.TimeRemaining <= 0)
        {
            GameInformation.Instance.GameOverResult = GameOverResult.Lose;
            GameInformation.Instance.GameState = GameState.PostGame;
            DeathCanvas.enabled = true;
        }
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
        {
            ScoreController.Instance.CalculateTimeRemainingScore();
            GameInformation.Instance.GameOverResult = GameOverResult.Win;
            GameInformation.Instance.GameState = GameState.PostGame;
        }
    }
}