using UnityEngine;
using System.Collections;

public class DroidInformation : EnemyInformation
{
    public float SeekPointRotateSpeed;
    public float SeekPlayerRotateSpeed;
    public float WalkSpeed;

    void Reset()
    {
        SeekPointRotateSpeed = 10;
        SeekPlayerRotateSpeed = 10;
        WalkSpeed = 50;
    }
}