using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingGoblin : Enemy
{
    

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        // 애니메이션 시작
        
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
        meleeArea.enabled= true;
        
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        meleeArea.enabled= false;
        
        yield return new WaitForSeconds(2f);       
        
        isChase = true;
        isAttack = false;
        // 애니메이션 끝
       
    }
}
