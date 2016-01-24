using UnityEngine;

public class PlayerInformation : CharacterInformation
{
    public bool GodMode;
    public bool DebugFire;

    public float RotationSpeed;
    public float Speed;

    void Reset()
    {
        Speed = 500;
    }

    void Awake()
    {
        CharacterStateChanged += PlayerInformation_CharacterStateChanged;
    }

    private void PlayerInformation_CharacterStateChanged(object sender, CharacterStateChangedEventArgs args)
    {
        if (GodMode)
            state = CharacterState.Alive;
    }
}