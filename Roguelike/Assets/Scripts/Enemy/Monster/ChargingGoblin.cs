using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargingGoblin : Enemy
{
    
    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        nav=GetComponent<NavMeshAgent>();
        anim=GetComponent<Animator>();
        material=GetComponentsInChildren<SkinnedMeshRenderer>();
        
        HitEffect = Resources.Load<GameObject>("HitEffect");
    }

    

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
        meleeArea.enabled= true;
        
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        meleeArea.enabled= false;
        
        yield return new WaitForSeconds(2f);       
        
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
       
    }
}
