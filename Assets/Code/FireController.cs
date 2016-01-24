using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(PlayerInformation))]
public class FireController : MonoBehaviour
{
    public GameObject FirePoint;
    public float FireCooldown;

    public int BounceLimit;

    private float cooldown;

    private GameObject firePoint;
    private PlayerInformation playerInformtion;
    private LineRenderer linerenderer;
    private float lineWidth = 0;

    void Start()
    {
        playerInformtion = GetComponent<PlayerInformation>();
        linerenderer = GetComponent<LineRenderer>();
        linerenderer.SetVertexCount(BounceLimit);

        firePoint = FirePoint;
    }

    void Update()
    {
        if (GameInformation.Instance.GameState != GameState.Playing) return;
        if (playerInformtion.CharacterState != CharacterState.Alive) return;

        cooldown += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && cooldown > FireCooldown)
        {
            cooldown = 0;

            List<Tuple<Vector3, Vector3>> points = CalculateFirePoints();
            CalculateEnemyHits(points);
            AnimateShot(points);

            CameraController.Instance.VisualEffectController.ChromaticAberration(40, 0.3f);
            CameraController.Instance.VisualEffectController.BlurredCorners(1, 0.3f);
        }
       
        if (lineWidth > 0)
        {
            lineWidth -= 0.1f;
            linerenderer.SetWidth(lineWidth, lineWidth);
        }
        else
        {
            lineWidth = 0;
            linerenderer.SetWidth(0, 0);
        }
        
    }

    /// <summary>
    /// Calculates all the fire points
    /// </summary>
    /// <returns>A list of point-heading tuples</returns>
    private List<Tuple<Vector3, Vector3>> CalculateFirePoints()
    {
        var firePoints = new List<Tuple<Vector3, Vector3>>();
        firePoints.Add(new Tuple<Vector3, Vector3>(firePoint.transform.position, transform.forward));

        firePoints = GetNextPointRecursivly(ref firePoints);

        return firePoints;
    }

    private List<Tuple<Vector3, Vector3>> GetNextPointRecursivly(ref List<Tuple<Vector3, Vector3>> currentPoints)
    {
        if (currentPoints.Count >= BounceLimit) return currentPoints;

        RaycastHit hit;
        if (Physics.Raycast(currentPoints.Last().Item1, currentPoints.Last().Item2, out hit, 1000, 1 << 12)) // HexWall 12
        {

            HexType hexType = hit.collider.GetComponentInParent<HexTileInformation>().HexType;
            Vector3 nextPointHeading = Vector3.zero;

            switch (hexType)
            {
                case HexType.Wall:
                    break;
                case HexType.Mirror:
                    nextPointHeading = Vector3.Reflect(currentPoints.Last().Item2, hit.normal);
                    break;
            }

            currentPoints.Add(new Tuple<Vector3, Vector3>(hit.point, nextPointHeading));

            if (nextPointHeading != Vector3.zero)
            {
                currentPoints = GetNextPointRecursivly(ref currentPoints);
            }
        }

        return currentPoints;
    }

    private void CalculateEnemyHits(List<Tuple<Vector3, Vector3>> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points.Count > i + 1) // is there a to-from point for a line
            {
                float rayDistance = Vector3.Distance(points[i].Item1, points[i].Item2);

                RaycastHit[] enemyHits = Physics.RaycastAll(points[i].Item1, points[i].Item2, rayDistance, 1 << 11); // Enemy layer 11

                foreach (var enemy in enemyHits)
                {
                    enemy.collider.GetComponentInParent<EnemyInformation>().CharacterState = CharacterState.Dead;
                }
            }
        }
    }
    private void AnimateShot(List<Tuple<Vector3, Vector3>> points)
    {
        lineWidth = 1f;
        linerenderer.SetWidth(lineWidth, lineWidth);
        for (int i = 0; i < points.Count; i++)
        {
            linerenderer.SetPosition(i, points[i].Item1);
        }
    }
}