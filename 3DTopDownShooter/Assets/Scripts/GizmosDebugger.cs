using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDebugger : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 5f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up * 5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }
}
