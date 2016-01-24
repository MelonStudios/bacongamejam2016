using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance;


}

public enum GameState
{
    PreGame,
    Playing,
    PostGame,
    GameOver
}