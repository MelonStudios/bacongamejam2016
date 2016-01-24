using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public float Speed;
    public PlayerState PlayerState;

    void Reset()
    {
        Speed = 500;
    }

    void Start()
    {
        PlayerState = PlayerState.Alive;
    }
}

public enum PlayerState
{
    Alive,
    Dead
}