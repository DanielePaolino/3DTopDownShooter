using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticPoolReturn : MonoBehaviour
{
    [SerializeField] private ObjectPooledType objectPooledType;
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), 2f);
    }

    private void ReturnToPool()
    {
        ObjectPoolerManager.Instance.ReturnToPool(this.gameObject, objectPooledType);
    }
}
