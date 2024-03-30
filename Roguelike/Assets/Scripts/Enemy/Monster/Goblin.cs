using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : Enemy
{


    public override IEnumerator Attack()
    {
        
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        
        yield return new WaitForSeconds(0.2f); // 공격 로직 시작
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f); // 공격 로직 끝
        meleeArea.enabled = false;


        yield return new WaitForSeconds(0.5f); 


        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);

    }





}
