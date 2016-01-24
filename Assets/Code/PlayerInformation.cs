using UnityEngine;

public class PlayerInformation : Character
{
    public float Speed;
    public bool GodMode;

    void Reset()
    {
        Speed = 500;
    }

    void Awake()
    {
        CharacterState = CharacterState.Alive;
        CharacterStateChanged += PlayerInformation_CharacterStateChanged;
    }

    private void PlayerInformation_CharacterStateChanged(object sender, CharacterStateChangedEventArgs args)
    {
        if (GodMode)
            state = CharacterState.Alive;
    }
}