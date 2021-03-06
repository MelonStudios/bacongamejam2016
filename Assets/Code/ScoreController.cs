﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;
    
    private int shotCount = 0;

    void Awake()
    {
        Instance = this;
        Score = 0;
        TimeCanvas.enabled = false;
    }

    #region Base Scores

    // General Scores
    public float SingleShot;

    // HexType Scores
    public float Wall;
    public float Mirror;

    // Time Scores
    public float SingleSecond;
    public float TenSeconds;
    public float ThirtySeconds;

    // Kill Scores
    public float TurretKill;
    public float DroidKill;

    #endregion

    #region Multipliers

    // HexType Multiplers
    public float TwoMirrorMulti;

    #endregion

    #region CanvasElements

    // Score Time Canvas
    public Canvas TimeCanvas;
    public Text TimeRemaining;
    public Text CurrentScore;
    public Text ShotsFired;
    public Text TenBonus;
    public Text ThirtyBonus;
    public Text FinalScore;

    #endregion

    public static float Score { get; private set; }

    public void CalculateFireScore(FireResults fireResults)
    {
        float tempScore = 0;
        float turretMulti = 1;
        float droidMulti = 1;
        float reflections = 1;
        float multiType = 0;

        foreach (var result in fireResults)
        {
            foreach (var enemy in result.HitEnemies)
            {
                if (enemy.GetComponentInParent<TurretInformation>() != null)
                {
                    if (turretMulti == 1) multiType++;

                    tempScore += AddScore(TurretKill * turretMulti * reflections, turretMulti + "turret kill");
                    turretMulti++;
                }
                else if (enemy.GetComponentInParent<DroidInformation>() != null)
                {
                    if (droidMulti == 1) multiType++;

                    tempScore += AddScore(DroidKill * droidMulti * reflections, droidMulti + "droid kill");
                    droidMulti++;
                }
            }

            Debug.Log(string.Format("After first shot: tempscore {0} | tmulti {1} | dmulti {2} | refl {3} | multype {4}", tempScore, turretMulti, droidMulti, reflections, multiType));

            if (result.EndingHexType == HexType.Mirror)
            {
                reflections++;
            }
        }

        if ((turretMulti > 1 || droidMulti > 1) && reflections > 1)
        {
            tempScore += AddScore(Mirror * reflections * ((reflections / 2) * TwoMirrorMulti), "mirror reflection");
        }

        if (multiType > 1)
        {
            tempScore = AddScore(tempScore * multiType, "multi enemy type x" + multiType + " bonus");
        }

        tempScore += AddScore(SingleShot, "single shot");
        shotCount++;

        Debug.LogWarning("Final score shot calc: " + tempScore);
        Score += tempScore;

        GameInformation.Instance.PlayerInformation.GetComponent<FireController>().AnimateShot(fireResults);
    }

    public void CalculateTimeRemainingScore()
    {
        StartCoroutine(DisplayTimeBasedScore());
    }
    private float AddScore(float amount, string reason)
    {
        Debug.Log("SCORECALC: " + amount + " " + reason);
        return amount;
    }

    void Update()
    {
        DebugController.Instance.LogLine("SCORE: " + Score);
    }

    IEnumerator DisplayTimeBasedScore()
    {
        float timeRemining = GameInformation.Instance.LevelInformation.TimeRemaining;

        float tempTimeRem = timeRemining;
        float canvasStart = Score;
        float canvasEnd = Score + Mathf.FloorToInt(timeRemining) * SingleSecond;

        TimeRemaining.text = Mathf.Floor(timeRemining).ToString();
        CurrentScore.text = Score.ToString();
        TenBonus.text = "10s Bonus: x" + Mathf.FloorToInt(timeRemining / 10);
        ThirtyBonus.text = "30s Bonus: x" + Mathf.FloorToInt(timeRemining / 30);

        TimeCanvas.enabled = true;

        yield return new WaitForSeconds(1);

        for (int i = 0; i < Mathf.FloorToInt(timeRemining) * 5; i++)
        {
            TimeRemaining.text = Mathf.Floor(Mathf.Lerp(timeRemining, 0, MathUtility.PercentageBetween(i, 0, Mathf.FloorToInt(timeRemining) * 5))).ToString();
            CurrentScore.text = Mathf.Lerp(canvasStart, canvasEnd, MathUtility.PercentageBetween(i, 0, Mathf.FloorToInt(timeRemining) * 5)).ToString();

            yield return new WaitForEndOfFrame();
        }

        TimeRemaining.text = "0";
        CurrentScore.text = canvasEnd.ToString();

        ShotsFired.text = shotCount.ToString();

        yield return new WaitForSeconds(0.6f);
        TenBonus.transform.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);
        ThirtyBonus.transform.gameObject.SetActive(true);

        // Actually calculate and add values
        float tempScore = 0;
        tempScore += AddScore(Mathf.FloorToInt(timeRemining) * SingleSecond, "single seconds");
        tempScore += AddScore(Mathf.FloorToInt(timeRemining / 10) * TenSeconds, "ten seconds");
        tempScore += AddScore(Mathf.FloorToInt(timeRemining / 30) * ThirtySeconds, "thirty seconds");
        Debug.LogWarning("Final score time calc: " + tempScore);

        yield return new WaitForSeconds(0.6f);
        FinalScore.transform.parent.gameObject.SetActive(true);

        FinalScore.text = (Score += tempScore).ToString();
    }

    public void RetryButton()
    {
        LevelController.RestartLevel();
    }

    public void NextLevelButton(string level)
    {
        LevelController.LoadLevel(level);
    }
}

public class FireResults : List<FireSegment>
{
    public int Bounces { get { return Count - 1; } }
}

public struct FireSegment
{
    public Vector3 Origin { get; private set; }
    public Vector3 Ending { get; private set; }
    public HexType EndingHexType { get; private set; }
    public List<GameObject> HitEnemies { get; private set; }

    public FireSegment(Vector3 origin, Vector3 ending, HexType endingHexType, List<GameObject> hitEnemies)
    {
        Origin = origin;
        Ending = ending;
        EndingHexType = endingHexType;
        HitEnemies = hitEnemies;
    }

    public Vector3 Direction { get { return (Ending - Origin).normalized; } }
}