using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : Enemy
{
    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetTrigger("doAttack");
        
        
        yield return new WaitForSeconds(0.8f); 
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.8f); 
        meleeArea.enabled = false;

        
        isChase = true;
        isAttack = false;
        

    }





}
