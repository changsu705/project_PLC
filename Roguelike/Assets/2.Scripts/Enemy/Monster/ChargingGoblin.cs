using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargingGoblin : Enemy
{

    private int force = 50;

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetTrigger("doCharge");
        
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.8f);
        rb.velocity = Vector3.zero;
        meleeArea.enabled = false;
        
        isChase = true;
        isAttack = false;
        
    }
}
