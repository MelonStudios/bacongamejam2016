﻿using UnityEngine;
using System.Collections;

public class PostGameController : MonoBehaviour
{
    public float SlowToStopTime;

    bool doOnce;

    void Start()
    {
        doOnce = true;
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.PostGame) return;

        if (doOnce)
        {
            doOnce = false;
            StartCoroutine(SlowGameToStop());
        }
    }

    IEnumerator SlowGameToStop()
    {
        float deltaTime = 0;
        float startingTimeScale = Time.timeScale;

        while (deltaTime <= SlowToStopTime)
        {
            deltaTime += Time.unscaledDeltaTime;

            Time.timeScale = Mathf.Lerp(startingTimeScale, 0, MathUtility.PercentageBetween(deltaTime, 0, SlowToStopTime));
            yield return new WaitForEndOfFrame();
        }

        GameInformation.Instance.GameState = GameState.GameOver;
    }
}