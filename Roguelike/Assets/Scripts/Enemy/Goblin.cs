
using System;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : Enemy
{
    
    

    

    

    public override void Targeting()
    {
       float targetRadius = 1.5f;
       float targetRange = 3f;
       
       RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
           LayerMask.GetMask("Player"));

    }

    
}
