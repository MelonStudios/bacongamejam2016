using UnityEngine;
using System.Collections;

public class PreGameController : MonoBehaviour
{
    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.PreGame) return;

        
        GameInformation.Instance.GameState = GameState.Playing;
    }
}