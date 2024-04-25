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
        anim.SetTrigger("doCharge");
        
        yield return new WaitForSeconds(1f);
        
        anim.SetTrigger("doAttack");
        GameObject instanceBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instanceBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;
        
        yield return new WaitForSeconds(1f);

        

        isChase = true;
        isAttack = false;
        
    }
}
