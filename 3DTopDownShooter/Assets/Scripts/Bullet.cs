using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 40f;
    private float maxDistance = 0.8f;
    private float distance = 0f;
    private int damage = 0;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        distance += 1 * Time.deltaTime;

        if (distance >= maxDistance)
            ObjectPoolerManager.Instance.ReturnToPool(gameObject, ObjectPooledType.Bullet);
    }
    private void OnEnable()
    {
        distance = 0f;
        damage = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Handle hit 
        IDamageable hit = other.gameObject.GetComponent<IDamageable>();
        if(hit != null)
        {
            hit.TakeDamage(damage);
        }
        ObjectPoolerManager.Instance.ReturnToPool(gameObject, ObjectPooledType.Bullet);

    }

    public void SetBulletDamage(int damage)
    {
        this.damage = damage;
    }

}
