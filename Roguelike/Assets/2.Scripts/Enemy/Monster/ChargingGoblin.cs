using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargingGoblin : Enemy
{

    private int force = 20;

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", true);
        

        yield return new WaitForSeconds(0.1f);
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.1f);

        isChase = true;
        isAttack = false;
        
        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack", false);

    }
}
