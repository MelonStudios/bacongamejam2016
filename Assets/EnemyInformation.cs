using UnityEngine;
using System.Collections;

public class EnemyInformation : Character
{
    void Awake()
    {
        CharacterState = CharacterState.Alive;
    }
}