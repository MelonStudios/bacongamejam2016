using UnityEngine;
using System.Collections;

public class PlayingController : MonoBehaviour
{
    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;

        if (GameInformation.Instance.PlayerInformation.PlayerState == PlayerState.Dead)
        {
            GameInformation.Instance.GameState = GameState.PostGame;
        }
    }
}