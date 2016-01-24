using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyInformation))]
public class DroidBehaviour : MonoBehaviour
{
    public GameObject DestroyedDroid;
    public GameObject Explosion;

    private EnemyInformation enemyInformation;

    void Start()
    {
        enemyInformation = GetComponent<EnemyInformation>();

        enemyInformation.CharacterStateChanged += EnemyInformation_CharacterStateChanged;
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