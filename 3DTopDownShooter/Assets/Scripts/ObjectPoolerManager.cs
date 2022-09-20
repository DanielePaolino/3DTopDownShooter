using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ObjectPooledType type;
        public GameObject prefab;
        public int size; //size of gameobjects in pool
    }

    public static ObjectPoolerManager Instance;

    [SerializeField] private List<Pool> pools = new List<Pool>();
    [SerializeField] private Dictionary<ObjectPooledType, Queue<GameObject>> objectPool;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        objectPool = new Dictionary<ObjectPooledType, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab);
                go.SetActive(false);
                queue.Enqueue(go);
            }

            objectPool.Add(pool.type, queue);
        }
    }

    public GameObject SpawnFromPool(ObjectPooledType type, Vector3 position, Quaternion rotation)
    {
        if (objectPool.ContainsKey(type))
        {
            if (objectPool[type].Count == 0)
            {
                AddToPool(type);
            }
            GameObject go = objectPool[type].Dequeue();
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);

            return go;

        }

        return null;
    }

    public GameObject SpawnFromPool(ObjectPooledType type, AudioClip audioclip, Vector3 position, Quaternion rotation)
    {
        if (objectPool.ContainsKey(type))
        {
            if (objectPool[type].Count == 0)
            {
                AddToPool(type);
            }
            GameObject go = objectPool[type].Dequeue();
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);

            return go;
        }

        return null;
    }

    public void ReturnToPool(GameObject go, ObjectPooledType type)
    {
        if (objectPool.ContainsKey(type))
        {
            go.SetActive(false);
            objectPool[type].Enqueue(go);
        }
    }

    private void AddToPool(ObjectPooledType type)
    {
        Pool p = pools.Find(t => t.type == type);
        if (p == null)
        {
            Debug.Log("No pool with type: " + type);
            return;
        }

        GameObject go = Instantiate(p.prefab);
        go.SetActive(false);
        objectPool[type].Enqueue(go);
    }
}
public enum ObjectPooledType
{
    Bullet, WeakEnemy, StrongEnemy, Boss, DeathParticleEffect
}