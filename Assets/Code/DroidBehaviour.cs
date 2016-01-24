using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyInformation), typeof(DroidPath))]
public class DroidBehaviour : MonoBehaviour
{
    public GameObject DestroyedDroid;
    public GameObject Explosion;

    public float MaxDistanceTriggerNextPoint;

    private DroidPath path;
    private EnemyInformation enemyInformation;

    void Start()
    {
        enemyInformation = GetComponent<EnemyInformation>();
        path = GetComponent<DroidPath>();

        enemyInformation.CharacterStateChanged += EnemyInformation_CharacterStateChanged;
    }

    void Update()
    {

    }

    private void EnemyInformation_CharacterStateChanged(object sender, CharacterStateChangedEventArgs args)
    {
        if (args.NewState == CharacterState.Dead)
        {
            enemyInformation.CharacterStateChanged -= EnemyInformation_CharacterStateChanged;
            Instantiate(DestroyedDroid, transform.position, transform.rotation);
            Instantiate(Explosion, transform.position,Quaternion.LookRotation(Vector3.up));
            Destroy(gameObject);
        }
    }
}