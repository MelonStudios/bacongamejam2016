using UnityEngine;

public class LevelInformation : MonoBehaviour
{
    public float TimeLimit;
    public float TimeRemaining;

    void Start() { TimeRemaining = TimeLimit; }
    void Update() { DebugController.Instance.LogLine("TIME REMINING: " + TimeRemaining); }
}