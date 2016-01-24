using UnityEngine;

public class PlayerLookColliderBehaviour : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player.gameObject != null)
        {
            transform.position = player.transform.position;
        }
    }
}