using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private KillableEntity entity;
    private float refreshPathTimer = 0.1f;
    private float chaseTimer = 0f;

    public ChaseState(KillableEntity entity) : base(entity.gameObject)
    {
        this.entity = entity;
    }
    public override Type Tick()
    {
        chaseTimer += Time.deltaTime;

        if(entity.Target == null)
            return typeof(WanderState);

        else
        {
            if(Vector3.Distance(entity.transform.position, entity.Target.position) <= entity.AttackDistance)
                return typeof(AttackState);
            if (chaseTimer > refreshPathTimer)
            {
                chaseTimer = 0f;
                entity.SetEntityDestination(entity.Target.position);
            }
                
        }


        return null;
    }
}
