using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] List<PathStation> patrolStations; 
    [SerializeField] EnemyTriggerZone triggerZone;
    [SerializeField] float triggerEnterDuration = 1f;
    [SerializeField] float triggerExitDuration = 1f;
    [SerializeField] float velocity = 2f;
    [SerializeField] Vector2 initialLookAt = Vector2.right;

    private Vector2 lookAt;
    private bool isTriggered;
    private bool isPatrolling;
    private bool canPatrol;
    private LayerMask triggerLayerMask;
    private RaycastHit2D[] playerHitArray;
    private PathStation nextStation;
    private CancellationTokenSource patrolCTS;

    private const float stationGizmoRadius = 0.2f;

    private void Awake()
    {
        patrolCTS = new CancellationTokenSource();
        foreach (var station in patrolStations)
        {
            station.SavePosition(transform);
        }
        lookAt = initialLookAt.normalized;
        canPatrol = patrolStations.Count >= 2 && !Mathf.Approximately((patrolStations[0].savedPosition - patrolStations[1].savedPosition).magnitude, 0);
        triggerLayerMask = Globals.PlayerLayerMask;
        playerHitArray = new RaycastHit2D[1];
        PatrolUntilTriggered();
    }

    private void FixedUpdate()
    {
        CheckTriggerZone();
    }

    private void CheckTriggerZone()
    {
        if(Physics2D.CircleCastNonAlloc(transform.position, triggerZone.range, lookAt, playerHitArray, 0, triggerLayerMask) > 0)
        {
            var playerPos = playerHitArray[0].point;
            var vecToPlayer = playerPos - (Vector2)transform.position;
            if(Mathf.Abs(Vector2.SignedAngle(lookAt, vecToPlayer)) < triggerZone.apexAngleDeg /2f)
            {
                Trigger();
            }
        }
    }


    private void Trigger()
    {
        isTriggered = true;
        isPatrolling = false;
        patrolCTS.Cancel();
        Debug.Log($"Enemy ({name}) triggered!");
    }

    private async UniTaskVoid PatrolUntilTriggered()
    {
        while(!isTriggered && canPatrol)
        {
            isPatrolling = true;
            patrolCTS.Token.ThrowIfCancellationRequested();
            foreach(var station in patrolStations)
            {
                nextStation = station;
                bool alreadyAtStation = Mathf.Approximately(((Vector3)nextStation.savedPosition - transform.position).magnitude, 0);
                if (!alreadyAtStation)
                {
                    lookAt = (nextStation.savedPosition - (Vector2)transform.position).normalized;
                    await MoveToNextStation(patrolCTS.Token);
                    await UniTask.WaitForSeconds(nextStation.stopDuration, cancellationToken: patrolCTS.Token);
                }
            }
        }
    }

    private async UniTask MoveToNextStation(CancellationToken token)
    {
        float duration = ((Vector3)nextStation.savedPosition - transform.position).magnitude / velocity;
        await transform.DOMove(nextStation.savedPosition, duration).ToUniTask(cancellationToken: token);
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            foreach (var station in patrolStations)
            {
                Gizmos.DrawWireSphere((Vector2)transform.position + station.relativePosition, stationGizmoRadius);
            }
            lookAt = initialLookAt.normalized;
        }
        if (!Application.isPlaying || DebugHelpers.ShowGizmosRunTime) 
        {
            var corners = triggerZone.GetCorners(lookAt);
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Debug.DrawLine(transform.position + (Vector3)corners[i], transform.position + (Vector3)corners[i + 1], Color.blue);
            }
            Debug.DrawLine(transform.position + (Vector3)corners[corners.Length - 1], transform.position + (Vector3)corners[0], Color.blue);
        }
    }

    private void OnDestroy()
    {
        patrolCTS.Dispose();
    }
}
