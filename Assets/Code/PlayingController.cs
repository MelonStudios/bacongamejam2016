using UnityEngine;
using System.Collections;

public class PlayingController : MonoBehaviour
{
    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;

        if (GameInformation.Instance.PlayerInformation.CharacterState == CharacterState.Dead)
        {
            GameInformation.Instance.GameOverResult = GameOverResult.Lose;
            GameInformation.Instance.GameState = GameState.PostGame;
        }
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
        {
            GameInformation.Instance.GameOverResult = GameOverResult.Win;
            GameInformation.Instance.GameState = GameState.PostGame;
        }
    }
}