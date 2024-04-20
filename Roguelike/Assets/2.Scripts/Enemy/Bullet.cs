using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public bool isMelee;

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
        
        
    }

    private void Update()
    {
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (!isMelee)
        {
            Destroy(gameObject, 2f);
        }
    }
}
