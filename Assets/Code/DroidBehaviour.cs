using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DroidInformation), typeof(DroidPath))]
public class DroidBehaviour : MonoBehaviour
{
    public GameObject DestroyedDroid;
    public GameObject Explosion;

    public float MaxDistanceTriggerNextPoint;
    public float ConeOfVisionAngle;

    private DroidPath path;
    private DroidInformation droidInformation;

    private float cooldown;

    private DroidMode mode;

    void Start()
    {
        droidInformation = GetComponent<DroidInformation>();
        path = GetComponent<DroidPath>();

        droidInformation.CharacterStateChanged += EnemyInformation_CharacterStateChanged;

        mode = DroidMode.SeekingPoint;
        StartCoroutine(SeekPoint(path.NextPoint));
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;
        if (droidInformation.CharacterState != CharacterState.Alive) return;

        cooldown += Time.deltaTime;

        switch (mode)
        {
            case DroidMode.Walking:
                {
                    if (PlayerIsVisible())
                    {
                        mode = DroidMode.SeekingPlayer;
                        StopAllCoroutines();
                        StartCoroutine(SeekPlayer());
                    }
                    else if (Vector3.Distance(transform.position, path.NextPoint) < MaxDistanceTriggerNextPoint)
                    {
                        mode = DroidMode.SeekingPoint;

                        StopAllCoroutines();
                        StartCoroutine(SeekPoint(path.GetAndSetNextPointIndex()));
                    }
                    else
                    {
                        transform.position += (path.NextPoint - transform.position).normalized * droidInformation.WalkSpeed * Time.deltaTime / 10;
                    }
                }
                break;
            case DroidMode.SeekingPoint:
                {
                    DebugController.Instance.LogLine("DROID ANGLE: " + Vector3.Angle(transform.forward, path.NextPoint - transform.position));
                    if (PlayerIsVisible())
                    {
                        mode = DroidMode.SeekingPlayer;
                        StopAllCoroutines();
                        StartCoroutine(SeekPlayer());
                    }
                    else if (Vector3.Angle(transform.forward, path.NextPoint - transform.position) < 0.8)
                    {
                        mode = DroidMode.Walking;

                        StopAllCoroutines();
                    }
                }
                break;
            case DroidMode.SeekingPlayer:
                {
                    if (!PlayerIsVisible())
                    {
                        mode = DroidMode.SeekingPoint;

                        StopAllCoroutines();
                        StartCoroutine(SeekPoint(path.NextPoint));
                    }
                    else if (CanFireOnPlayer())
                    {
                        Fire();
                    }
                }
                break;
            default:
                break;
        }
    }

    private bool PlayerIsVisible()
    {
        if (Vector3.Angle(transform.forward, GameInformation.Instance.PlayerInformation.transform.position - transform.position) < ConeOfVisionAngle)
        {
            Ray toPlayerRay = new Ray(transform.position, (GameInformation.Instance.PlayerInformation.transform.position - transform.position).normalized);
            float toPlayerDist = Vector3.Distance(transform.position, GameInformation.Instance.PlayerInformation.transform.position);

            RaycastHit hit;
            if (!Physics.Raycast(toPlayerRay, out hit, toPlayerDist, 1 << 12)) // HexWall layer 12
            {
                return true;
            }
        }

        return false;
    }

    private bool CanFireOnPlayer()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit);

        return hit.collider != null && hit.collider.tag == "Player";
    }

    private void Fire()
    {
        if (cooldown > droidInformation.FireCooldown)
        {
            cooldown = 0;

            foreach (var audioClip in droidInformation.FireSounds)
            {
                Camera.main.GetComponent<AudioSource>().PlayOneShot(audioClip);
            }
            
            Instantiate(droidInformation.Bullet, droidInformation.FirePoints[Random.Range(0, droidInformation.FirePoints.Length - 1)].transform.position, transform.rotation);
        }
    }

    private void EnemyInformation_CharacterStateChanged(object sender, CharacterStateChangedEventArgs args)
    {
        if (args.NewState == CharacterState.Dead)
        {
            droidInformation.CharacterStateChanged -= EnemyInformation_CharacterStateChanged;
            Instantiate(DestroyedDroid, transform.position, transform.rotation);
            Instantiate(Explosion, transform.position, Quaternion.LookRotation(Vector3.up));
            Destroy(gameObject);
        }
    }

    IEnumerator SeekPoint(Vector3 point)
    {
        while (true)
        {
            Vector3 targetDir = point - transform.position;
            float step = droidInformation.SeekPointRotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            newDir = new Vector3(newDir.x, 0, newDir.z);
            transform.rotation = Quaternion.LookRotation(newDir);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SeekPlayer()
    {
        while (true)
        {
            Vector3 targetDir = GameInformation.Instance.PlayerInformation.transform.position - transform.position;
            float step = droidInformation.SeekPointRotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            newDir = new Vector3(newDir.x, 0, newDir.z);
            transform.rotation = Quaternion.LookRotation(newDir);

            yield return new WaitForEndOfFrame();
        }
    }

    public enum DroidMode
    {
        Walking,
        SeekingPoint,
        SeekingPlayer
    }
}