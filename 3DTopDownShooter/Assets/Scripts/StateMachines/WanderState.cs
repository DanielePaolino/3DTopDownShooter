using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WanderState : State
{
    private KillableEntity entity;
    private float timer = 0f;
    private float wanderTime = 1f;
    private float wanderRadius = 10f;

    public WanderState(KillableEntity entity) : base(entity.gameObject)
    {
        this.entity = entity;
    }

    public override Type Tick()
    {
        if (CanSeePlayer())
            return typeof(ChaseState);

        timer += Time.deltaTime;

        if (timer >= wanderTime)
        {
            RandomDestination();        
            timer = 0f;
        }

        return null;
    }

    private void RandomDestination()
    {
        Vector3 randomDestination = Random.insideUnitSphere * wanderRadius;
        randomDestination.y = entity.transform.position.y;
        randomDestination += entity.transform.position;

        NavMeshHit navHit;

        if(NavMesh.SamplePosition(randomDestination, out navHit, wanderRadius, -1))
        {
            entity.SetEntityDestination(navHit.position);
        }
        else
        {
            NavMesh.SamplePosition(-randomDestination, out navHit, wanderRadius, -1);
            entity.SetEntityDestination(navHit.position);
        }
    }

    private bool CanSeePlayer()
    {
        if (entity.Target == null)
            return false;

        if (Vector3.Distance(entity.transform.position, entity.Target.position) > entity.ViewDistance)
            return false;

        Vector3 directionToTarget = (entity.Target.position - entity.transform.position).normalized;
        if(Vector3.Angle(entity.transform.forward, directionToTarget) < entity.AngleOfView / 2)
        {
            return true;
        }

        return false;
    }
}
