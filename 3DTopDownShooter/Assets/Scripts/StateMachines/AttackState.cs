using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private KillableEntity entity;
    private float attackTimer = 0f;
    public AttackState(KillableEntity entity) : base(entity.gameObject)
    {
        this.entity = entity;
    }
    public override Type Tick()
    {
        attackTimer += Time.deltaTime;

        if (entity.Target == null)
            return typeof(WanderState);

        if (Vector3.Distance(entity.transform.position, entity.Target.position) <= entity.AttackDistance)
        {
            entity.SetEntityDestination(entity.Target.position);

            if (attackTimer > entity.FireRate)
            {
                entity.Attack();
            }
            
        }
        else
        {
            return typeof(ChaseState);
        }

        return null;
    }
}
