using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LongRangeGoblin : Enemy
{
    public float bulletSpeed = 20f;
    
    public GameObject bullet;
    public Transform bulletPos;
    

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetBool("isCharge",true);
        
        yield return new WaitForSeconds(1f);
        
        
        anim.SetBool("isCharge",false);
        anim.SetBool("isAttack", true);
        GameObject instanceBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instanceBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;
        

        yield return new WaitForSeconds(0.7f);

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        anim.SetBool("isWalk", true);
    }
}
