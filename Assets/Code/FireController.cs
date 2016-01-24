using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

[RequireComponent(typeof(PlayerInformation))]
public class FireController : MonoBehaviour
{
    public float FireCooldown;

    public int BounceLimit;

    private float cooldown;
    
    private PlayerInformation playerInformtion;
    private LineRenderer lineRenderer;
    private float lineWidth = 0;

    void Start()
    {
        playerInformtion = GetComponent<PlayerInformation>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(BounceLimit);
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;
        if (playerInformtion.CharacterState != CharacterState.Alive) return;

        cooldown += Time.deltaTime;

        if (playerInformtion.DebugFire)
        {
            if (Input.GetButton("Fire1"))
            {
                FireResults points = CalculateFirePoints();

                foreach (var point in points)
                {
                    Debug.DrawLine(point.Origin, point.Ending, Color.red);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && cooldown > FireCooldown)
            {
                cooldown = 0;

                FireResults points = CalculateFirePoints();
                ScoreController.Instance.CalculateFireScore(points);

                CameraController.Instance.VisualEffectController.ChromaticAberration(40, 0.3f);
                CameraController.Instance.VisualEffectController.BlurredCorners(1, 0.3f);
            }

            if (Input.GetMouseButtonDown(1))
            {
                LevelController.RestartLevel();
            }
        }
    }

    /// <summary>
    /// Calculates all the fire points
    /// </summary>
    /// <returns>A list of point-heading tuples</returns>
    private FireResults CalculateFirePoints()
    {
        var firePoints = new FireResults();
        firePoints = GetNextPointRecursivly(ref firePoints, playerInformtion.FirePoints[0].transform.position, transform.forward);

        return firePoints;
    }

    private FireResults GetNextPointRecursivly(ref FireResults currentFireResults, Vector3 origin, Vector3 heading)
    {
        if (currentFireResults.Bounces >= BounceLimit) return currentFireResults;

        RaycastHit hit;
        if (Physics.Raycast(origin, heading, out hit, 1000, 1 << 12)) // HexWall 12
        {
            HexType hexType = hit.collider.GetComponentInParent<HexTileInformation>().HexType;
            List<GameObject> hitEnemies = CalculateEnemyHits(origin, hit.point, heading);

            currentFireResults.Add(new FireSegment(origin, hit.point, hexType, hitEnemies));

            switch (hexType)
            {
                case HexType.Wall:
                    break;
                case HexType.Mirror:
                    currentFireResults = GetNextPointRecursivly(ref currentFireResults, hit.point, Vector3.Reflect(heading, hit.normal));
                    break;
            }
        }

        return currentFireResults;
    }

    private List<GameObject> CalculateEnemyHits(Vector3 origin, Vector3 ending, Vector3 direction)
    {
        float rayDistance = Vector3.Distance(origin, ending);
        return Physics.RaycastAll(origin, direction, rayDistance, 1 << 11).Select(rh => rh.collider.gameObject).ToList(); // Enemy layer 11
    }

    public void AnimateShot(FireResults fireResults)
    {
        lineRenderer.SetVertexCount(fireResults.Count * 2);
        lineWidth = 1f;
        lineRenderer.SetWidth(lineWidth, lineWidth);

        for (int i = 0; i < fireResults.Count; i++)
        {
            lineRenderer.SetPosition(i * 2, fireResults[i].Origin);
            lineRenderer.SetPosition(i * 2 + 1, fireResults[i].Ending);
        }

        StartCoroutine(AnimateLineRendererOut());
    }

    IEnumerator AnimateLineRendererOut()
    {
        while (lineWidth > 0)
        {
            lineWidth -= 0.1f;
            lineRenderer.SetWidth(lineWidth, lineWidth);
            yield return new WaitForEndOfFrame();
        }

        lineWidth = 0;
        lineRenderer.SetWidth(0, 0);
    }
}