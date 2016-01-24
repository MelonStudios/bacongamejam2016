using UnityEngine;
using System.Collections;

public class EnemyInformation : MonoBehaviour
{
    public EnemyState EnemyState;
}

public enum EnemyState
{
    Alive,
    Dead
}