using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;


    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.CompareTag("Player"))
        {
            print("초아아랄");
            Destroy(gameObject);
        }
        else if(!isMelee)
        {
            Destroy(gameObject, 3f);
        }
    }
}
