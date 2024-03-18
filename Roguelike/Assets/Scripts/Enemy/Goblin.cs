
using System.Collections;
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

        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
            
        }

    }

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        // 애니메이션

        yield return new WaitForSeconds(0.2f); // 공격 로직 시작
        meleeArea.enabled= true;
        
        yield return new WaitForSeconds(0.5f); // 공격 로직 끝
        meleeArea.enabled= false;
        

        yield return new WaitForSeconds(1f); // 1초간 대기
        
        
        isChase= true;
        isAttack = false;
        // 애니메이션 끝

    }
    

   

    
}
