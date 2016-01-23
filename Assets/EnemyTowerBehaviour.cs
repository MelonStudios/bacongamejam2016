using System.Collections;
using UnityEngine;

public class EnemyTowerBehaviour : MonoBehaviour
{
    public GameObject TowerGun;
    public GameObject Bullet;
    public GameObject FirePoint;

    public float IdleRotateSpeed;
    public float SeekRotateSpeed;

    public float FireCooldown;

    private GameObject player;
    private TurretMode mode;

    private float cooldown;

    private Coroutine activeCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mode = TurretMode.Idle;

        activeCoroutine = StartCoroutine(IdleRotate());
    }

    void Update()
    {
        cooldown += Time.deltaTime;

        switch (mode)
        {
            case TurretMode.Idle:
                {
                    if (CanSeePlayer())
                    {
                        mode = TurretMode.SeekingPlayer;

                        StopCoroutine(activeCoroutine);
                        activeCoroutine = StartCoroutine(SeekPlayer());
                    }
                }
                break;
            case TurretMode.SeekingPlayer:
                {
                    if (!CanSeePlayer())
                    {
                        mode = TurretMode.Idle;

                        StopCoroutine(activeCoroutine);
                        activeCoroutine = StartCoroutine(IdleRotate());
                    }

                    if (CanFireOnPlayer())
                    {
                        Fire();
                    }
                }
                break;
            default:
                break;
        }
    }

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        Physics.Raycast(TowerGun.transform.position, player.transform.position - TowerGun.transform.position, out hit);
        
        return hit.collider != null && hit.collider.tag == "Player";
    }

    private bool CanFireOnPlayer()
    {
        RaycastHit hit;
        Physics.Raycast(TowerGun.transform.position, TowerGun.transform.forward, out hit);

        return hit.collider != null && hit.collider.tag == "Player";
    }

    private void Fire()
    {
        if (cooldown > FireCooldown)
        {
            cooldown = 0;

            Instantiate(Bullet, FirePoint.transform.position, TowerGun.transform.rotation);
        }
    }

    IEnumerator IdleRotate()
    {
        while (true)
        {
            TowerGun.transform.Rotate(Vector3.up, IdleRotateSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SeekPlayer()
    {
        while (true)
        {
            Vector3 targetDir = player.transform.position - TowerGun.transform.position;
            float step = SeekRotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(TowerGun.transform.forward, targetDir, step, 0.0F);
            newDir = new Vector3(newDir.x, 0, newDir.z);
            TowerGun.transform.rotation = Quaternion.LookRotation(newDir);

            yield return new WaitForEndOfFrame();
        }
    }

    public enum TurretMode
    {
        Idle,
        SeekingPlayer
    }
}