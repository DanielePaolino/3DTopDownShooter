using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private Transform[] spawnPositions;
    private float timer = 0f;
    private float spawnWaitingTime = 1f;
    private int enemiesSpawnedCounter = 0;
    private float medkitTimer = 0f;
    private float medkitWaitingTime = 20f;
    private bool canSpawnMedkit = true;

    // Start is called before the first frame update
    void Start()
    {
        spawnWaitingTime += Random.Range(0f, 1.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        medkitTimer += Time.deltaTime;
        if (timer > spawnWaitingTime)
        {
            SpawnEntity();
            timer = 0f;
            spawnWaitingTime = 1.5f + Random.Range(0f, 1.5f);
        }
        if(medkitTimer > medkitWaitingTime && canSpawnMedkit)
        {
            SpawnMedkit();         
        }
    }

    private void OnEnable()
    {
        Medkit.OnMedkitUse += CanSpawnMedkit;
    }

    private void OnDisable()
    {
        Medkit.OnMedkitUse -= CanSpawnMedkit;
    }

    public void SpawnEntity()
    {
        enemiesSpawnedCounter++;
        ObjectPooledType entity;

        if (enemiesSpawnedCounter > 10)
        {
            entity = ObjectPooledType.Boss;
            enemiesSpawnedCounter = 0;
        }
          
        else
            entity = Random.Range(0, 10) > 7 ? ObjectPooledType.StrongEnemy : ObjectPooledType.WeakEnemy;

        Vector3 randomPos;
        randomPos.y = 1f;
        Vector3 spawnPos = Vector3.zero;
        NavMeshHit navHit;
        int counter = 0;
        while(true)
        {
            //a very simple method to avoid a long loop
            if(counter > 5)
            {
                spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
                break;
            }
            randomPos = Random.insideUnitSphere * 60f;
            randomPos.y = 1f;
            if (NavMesh.SamplePosition(randomPos, out navHit, 30f, -1))
            {               
                spawnPos = navHit.position;
                break;
            }
            counter++;
           
        }
        ObjectPoolerManager.Instance.SpawnFromPool(entity, spawnPos, Quaternion.identity);
       
    }

    private void SpawnMedkit()
    {
        Vector3 randomPos;
        randomPos.y = 1f;
        Vector3 spawnPos = Vector3.zero;
        NavMeshHit navHit;
        int counter = 0;
        while(true)
        {
            //a very simple method to avoid a long loop
            if (counter > 5)
            {
                spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
                break;
            }

            randomPos = Random.insideUnitSphere * 60f;
            randomPos.y = 1f;
            if (NavMesh.SamplePosition(randomPos, out navHit, 30f, -1))
            {               
                spawnPos = navHit.position;
                spawnPos.y = 2;
                break;
            }

            counter++;
           
        }
        Instantiate(medkitPrefab, spawnPos, Quaternion.identity);
        canSpawnMedkit = false;
    }

    void CanSpawnMedkit()
    {
        medkitTimer = 0f;
        canSpawnMedkit = true;       
    }


}
