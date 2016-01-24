using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DroidInformation), typeof(DroidPath))]
public class DroidBehaviour : MonoBehaviour
{
    public GameObject DestroyedDroid;
    public GameObject Explosion;

    public float MaxDistanceTriggerNextPoint;

    private DroidPath path;
    private DroidInformation droidInformation;

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

        switch (mode)
        {
            case DroidMode.Walking:
                {
                    if (Vector3.Distance(transform.position, path.NextPoint) < MaxDistanceTriggerNextPoint)
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
                    Debug.Log(Vector3.Angle(transform.forward, path.NextPoint - transform.position));
                    if (Mathf.Approximately(Vector3.Angle(transform.forward, path.NextPoint - transform.position), 0))
                    {
                        mode = DroidMode.Walking;

                        StopAllCoroutines();
                    }
                }
                break;
            case DroidMode.SeekingPlayer:
                break;
            default:
                break;
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

    public enum DroidMode
    {
        Walking,
        SeekingPoint,
        SeekingPlayer
    }
}