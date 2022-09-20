using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : KillableEntity
{
    private float fireTimer = 1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetValues(10, 0.4f, 7f, 180f, 2, 0f, 20f, 30f, 20f);
    }

    protected override void InitializeStateMachine()
    {
        Dictionary<Type, State> states = new Dictionary<Type, State>()
        {
            { typeof(ChaseState), new ChaseState(this) },
            { typeof(AttackState), new AttackState(this) }

        };
        stateMachine.SetStates(states);
    }

    public override void Attack()
    {

        fireTimer += Time.deltaTime;
        if (fireTimer > FireRate)
        {
            fireTimer = 0f;
            Bullet bullet;
            ObjectPoolerManager.Instance.SpawnFromPool(ObjectPooledType.Bullet, firePos.position, transform.rotation).TryGetComponent<Bullet>(out bullet);
            if (bullet == null)
            {
                Debug.LogError("StrongEnemy - Attack - Error from spawning bullet from pool");
                return;
            }

            bullet.SetBulletDamage(damage);
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    protected override void Death()
    {
        base.Death();
        ObjectPoolerManager.Instance.ReturnToPool(this.gameObject, ObjectPooledType.StrongEnemy);
    }

}
