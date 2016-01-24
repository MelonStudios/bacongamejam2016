using UnityEngine;
using System.Collections;

public class PostGameController : MonoBehaviour
{
    public float SlowToStopTime;
    public float LowestTimeScale;

    public GameObject Explosion;
    public GameObject PlayerGib;
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
            for(int i = 0; i<3; i++)
            { 
                GameObject playerGib = Instantiate(PlayerGib, GameInformation.Instance.PlayerInformation.gameObject.transform.position + Vector3.up*(i), Quaternion.identity) as GameObject;
                playerGib.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-400f, 400f), Random.Range(-400f, 400f), Random.Range(-400f, 400f)));
                playerGib.GetComponent<Rigidbody>().AddTorque(Vector3.one *Random.Range(-500f,500f));
            }
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