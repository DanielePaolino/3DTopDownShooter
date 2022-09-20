using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class KillableEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected Transform firePos;
    protected int maxHealth;
    protected int health;
    public float FireRate { get; protected set; }
    protected float movementSpeed;
    protected float rotationSpeed;
    protected int damage;
    public float AngleOfView { get; protected set; }
    public float FireAngle { get; protected set; }
    public float ViewDistance { get; protected set; }
    public float AttackDistance { get; protected set; }
    protected bool isDead;
    protected NavMeshAgent agent;
    protected StateMachine stateMachine;
    public Transform Target { get; protected set; }
    protected HealthBarUI healthBarUI;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        InitializeStateMachine();
    }

    protected virtual void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Physics.IgnoreCollision(GetComponent<Collider>(), Target.gameObject.GetComponent<Collider>());
        healthBarUI = GetComponent<HealthBarUI>();
    }

    private void OnEnable()
    {
        try
        {
            if (healthBarUI == null)
                healthBarUI = GetComponent<HealthBarUI>();

            if (Target == null)
                Target = GameObject.FindGameObjectWithTag("Player").transform;

            Physics.IgnoreCollision(GetComponent<Collider>(), Target.gameObject.GetComponent<Collider>());
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e);
        }
      
    }

    protected void OnDisable()
    {
        ResetKillableEntity();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBarUI.UpdateHealthBar(health, maxHealth);
 
        if (health <= 0 && !isDead)
        {
            Death();
        }
    }

    protected void SetValues(int _maxHealth, float _fireRate, float _movementSpeed, float _rotationSpeed, int _damage, float _angleOfView, float _fireAngle, float _viewDistance, float _attackDistance)
    {
        maxHealth = _maxHealth;
        health = _maxHealth;
        FireRate = _fireRate;
        movementSpeed = _movementSpeed;
        rotationSpeed = _rotationSpeed;
        damage = _damage;
        agent.speed = movementSpeed;
        agent.angularSpeed = rotationSpeed;
        AngleOfView = _angleOfView;
        FireAngle = _fireAngle;
        ViewDistance = _viewDistance;
        AttackDistance = _attackDistance;
    }

    protected virtual void Death()
    {
        isDead = true;
        ObjectPoolerManager.Instance.SpawnFromPool(ObjectPooledType.DeathParticleEffect, transform.position, transform.rotation);
    }

    private void ResetKillableEntity()
    {
        health = maxHealth;
        isDead = false;
    }

    public void SetEntityDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public bool EntityHasPath()
    {
        return agent.hasPath;
    }

    protected abstract void InitializeStateMachine();
    public abstract void Attack();

}
