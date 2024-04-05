using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : Enemy
{


    public override IEnumerator Attack()
    {
        
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", true);
        
        yield return new WaitForSeconds(0.14f); 
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.02f); 
        meleeArea.enabled = false;


        yield return new WaitForSeconds(1.1f); 

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        anim.SetBool("isWalk", true);

    }





}
