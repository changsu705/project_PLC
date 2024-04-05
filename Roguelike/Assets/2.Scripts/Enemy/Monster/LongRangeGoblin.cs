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
        // 애니메이션 시작

        yield return new WaitForSeconds(0.5f);
        GameObject instanceBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instanceBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;

        yield return new WaitForSeconds(2f);

        isChase = true;
        isAttack = false;
        // 애니메이션 끝

    }
}
