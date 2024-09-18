using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Entity 
{
    [SerializeField] List<PathStation> patrolStations; 
    [SerializeField] EnemyTriggerZone triggerZone;
    [SerializeField] float triggerExitDuration = 5f;
    [SerializeField] float velocity = 2f;
    [SerializeField] Vector2 initialLookAt = Vector2.right;

    private Vector2 lookAt;
    private bool isTriggered;
    private bool isPatrolling;
    private bool canPatrol;
    private ContactFilter2D triggerContactFilter;
    private Collider2D[] playerColliderHitArray;
    private PathStation nextStation;
    private CancellationTokenSource patrolCTS;

    private const float HitDuration = .5f;
    private const float StationGizmoRadius = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        patrolCTS = new CancellationTokenSource();
        foreach (var station in patrolStations)
        {
            station.SavePosition(transform);
        }
        lookAt = initialLookAt.normalized;
        canPatrol = patrolStations.Count >= 2 && !Mathf.Approximately((patrolStations[0].savedPosition - patrolStations[1].savedPosition).magnitude, 0);
        triggerContactFilter = new ContactFilter2D();
        triggerContactFilter.SetLayerMask(Globals.PlayerLayerMask);
        playerColliderHitArray = new Collider2D[1];
        PatrolUntilTriggered();
    }

    private void FixedUpdate()
    {
        if (!isTriggered)
        {
            CheckTriggerZone();
        }

    }

    public override void TakeDamage(int damage, Vector2 hitVector)
    {
        if(!IsAlive)
        {
            return;
        }
        base.TakeDamage(damage, hitVector);
        transform.DOMove((Vector2)transform.position + hitVector, HitDuration).ToUniTask().Forget();
    }

    #region Triggering

    private void CheckTriggerZone()
    {
        if (Physics2D.OverlapCircle(transform.position, triggerZone.range, triggerContactFilter, playerColliderHitArray) > 0)
        {
            var playerPos = playerColliderHitArray[0].transform.position;
            var vecToPlayer = playerPos - transform.position;
            if (Mathf.Abs(Vector2.SignedAngle(lookAt, vecToPlayer)) < triggerZone.apexAngleDeg / 2f)
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

    #endregion

    #region Partolling

    private async UniTaskVoid PatrolUntilTriggered()
    {
        while (!isTriggered && canPatrol)
        {
            isPatrolling = true;
            patrolCTS.Token.ThrowIfCancellationRequested();
            foreach (var station in patrolStations)
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

    #endregion

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            foreach (var station in patrolStations)
            {
                Gizmos.DrawWireSphere((Vector2)transform.position + station.relativePosition, StationGizmoRadius);
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
