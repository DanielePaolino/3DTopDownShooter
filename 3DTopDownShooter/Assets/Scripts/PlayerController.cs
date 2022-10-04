using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region Fields

    public static PlayerController Instance { get; private set; }

    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject playerCanvas;

    [Header("Movement Variables")]
    [Tooltip("Player Speed")]
    [SerializeField] private float speed;

    [Space]
    [Header("Stats")]
    [SerializeField] private int playerFireDamage = 1;
    [SerializeField] private int health = 10;
    [SerializeField] private int maxHealth = 10;

    private PlayerInputActions InputActions;
    private HealthBarUI healthBarUI;
    private Rigidbody rBody;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 move = Vector3.zero;
    private bool fireInput = false;
    private bool playerIsDead = false;
    private float fireTimer = 1f;
    private float playerFireRate = 0.15f;

    #endregion Fields


    private void Awake()
    {
        Singleton();
        HandleInput();
    }
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        healthBarUI = GetComponent<HealthBarUI>();
        playerIsDead = false;
        health = maxHealth;
    }

    void Update()
    {
        if(!playerIsDead)
        {
            RotateTowardsMouse();
            Fire();
        }
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (moveInput != Vector2.zero && !playerIsDead)
        {
            move = new Vector3(moveInput.x, 0f, moveInput.y);
            rBody.MovePosition(rBody.position + move * speed * Time.fixedDeltaTime);
        }
    }

    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    private void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(lookInput);
        Plane plane = new Plane(transform.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            hitPoint.y = transform.position.y;

            Vector3 direction = (hitPoint - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction, transform.up);
        }
    }

    private void Fire()
    {
        fireTimer += Time.deltaTime;
        if(fireInput && fireTimer > playerFireRate)
        {
            fireTimer = 0f;
            Bullet bullet;
            ObjectPoolerManager.Instance.SpawnFromPool(ObjectPooledType.Bullet, firePos.position, transform.rotation).TryGetComponent<Bullet>(out bullet);
            if(bullet == null)
            {
                Debug.LogError("PlayerController - Fire - Error spawning bullet from pool");
                return;
            }

            bullet.SetBulletDamage(playerFireDamage);
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    public void TakeDamage(int damage)
    {       
        health -= damage;
        healthBarUI.UpdateHealthBar(health, maxHealth);
        if (health <= 0 && !playerIsDead)
        {
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        Debug.Log("Player id dead!");
        playerIsDead = true;
        ObjectPoolerManager.Instance.SpawnFromPool(ObjectPooledType.DeathParticleEffect, transform.position, transform.rotation);
        GetComponent<MeshRenderer>().enabled = false;
        playerCanvas.SetActive(false);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    public void RestoreHealth()
    {
        if(!playerIsDead)
        {
            health = maxHealth;
            healthBarUI.UpdateHealthBar(health, maxHealth);
        }     
    }

    private void HandleInput()
    {
        InputActions = new PlayerInputActions();

        InputActions.Player.Movement.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
        InputActions.Player.Movement.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };

        InputActions.Player.Fire.performed += ctx =>
        {
            fireInput = true;
        };

        InputActions.Player.Fire.canceled += ctx =>
        {
            fireInput = false;
        };

        InputActions.Player.Looking.performed += ctx =>
        {
            lookInput = ctx.ReadValue<Vector2>();
        };
    }

    private void Singleton()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
