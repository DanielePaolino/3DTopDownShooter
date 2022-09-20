using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public static event Action OnMedkitUse;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().RestoreHealth();
            if(OnMedkitUse != null)
                OnMedkitUse();
            Destroy(gameObject);
        }
    }
}
