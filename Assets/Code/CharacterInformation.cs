using UnityEngine;

public class CharacterInformation : MonoBehaviour
{
    protected CharacterState state;

    public CharacterState CharacterState
    {
        get { return state; }
        set
        {
            CharacterState oldState = state;
            state = value;

            if (CharacterStateChanged != null)
                CharacterStateChanged.Invoke(this, new CharacterStateChangedEventArgs(oldState, state));
        }
    }

    public delegate void CharacterStateChangedEventHandler(object sender, CharacterStateChangedEventArgs args);
    public event CharacterStateChangedEventHandler CharacterStateChanged;

    void Awake()
    {
        CharacterState = CharacterState.Alive;
    }
}

public class CharacterStateChangedEventArgs
{
    public CharacterStateChangedEventArgs(CharacterState oldState, CharacterState newState)
    {
        OldState = oldState;
        NewState = newState;
    }

    public CharacterState OldState { get; set; }
    public CharacterState NewState { get; set; }
}

public enum CharacterState
{
    Alive,
    Dead
}