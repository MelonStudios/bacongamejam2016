using UnityEngine;
using System.Collections;

public class GameMusicController : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<GameMusicController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}