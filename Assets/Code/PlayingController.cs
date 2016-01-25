using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayingController : MonoBehaviour
{
    public Canvas DeathCanvas;
    public Canvas HUD;
    public Text timer;

    void Awake()
    {
        DeathCanvas.enabled = false;
        GUI.enabled = true;
    }

    void Start()
    {
        timer.text = GameInformation.Instance.LevelInformation.TimeLimit.ToString();
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;

        HUD.enabled = true;

        GameInformation.Instance.LevelInformation.TimeRemaining -= Time.deltaTime;
        timer.text = Mathf.Round(GameInformation.Instance.LevelInformation.TimeRemaining).ToString();
        
        
        if (GameInformation.Instance.PlayerInformation.CharacterState == CharacterState.Dead ||
            GameInformation.Instance.LevelInformation.TimeRemaining <= 0)
        {
            GameInformation.Instance.GameOverResult = GameOverResult.Lose;
            GameInformation.Instance.GameState = GameState.PostGame;
            DeathCanvas.enabled = true;
            HUD.enabled = false;
        }
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
        {
            ScoreController.Instance.CalculateTimeRemainingScore();
            GameInformation.Instance.GameOverResult = GameOverResult.Win;
            GameInformation.Instance.GameState = GameState.PostGame;
            HUD.enabled = false;
        }
    }
}