using UnityEngine;
using System.Collections;

public class PostGameController : MonoBehaviour
{
    public float SlowToStopTime;
    public float LowestTimeScale;

    public GameObject Explosion;

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
            Debug.Log("GAME OVER: " + GameInformation.Instance.GameOverResult);

            doOnce = false;
            Instantiate(Explosion, GameInformation.Instance.PlayerInformation.gameObject.transform.position, Quaternion.LookRotation(Vector3.up));
            Destroy(GameInformation.Instance.PlayerInformation.gameObject);
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
            if (Time.timeScale < LowestTimeScale)
            {
                Time.timeScale = LowestTimeScale;
            }
            yield return new WaitForEndOfFrame();
        }

        GameInformation.Instance.GameState = GameState.GameOver;
    }
}