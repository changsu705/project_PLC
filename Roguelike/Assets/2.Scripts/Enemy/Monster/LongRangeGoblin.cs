using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LongRangeGoblin : Enemy
{
    public float bulletSpeed = 5f;
    
    public GameObject bullet;
    public Transform bulletPos;
    

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        //anim.SetBool("isCharge",true);
        anim.SetBool("isAttack", true);
        
        
        
        //anim.SetBool("isCharge",false);
        
        GameObject instanceBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instanceBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;
        
        yield return new WaitForSeconds(2f);

        

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        anim.SetBool("isWalk", true);
        
        
    }
}
