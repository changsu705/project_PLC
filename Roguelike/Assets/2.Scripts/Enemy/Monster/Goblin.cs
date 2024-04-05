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
        
        yield return new WaitForSeconds(0.9f); 
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.3f); 
        meleeArea.enabled = false;


        yield return new WaitForSeconds(1f); 


        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        anim.SetBool("isWalk", true);

    }





}
