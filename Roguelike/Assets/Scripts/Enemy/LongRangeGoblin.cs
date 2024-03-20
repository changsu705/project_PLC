using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LongRangeGoblin : Enemy
{

    public GameObject bullet;
    public Transform bulletPos;

    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        nav=GetComponent<NavMeshAgent>();
    }

    public override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        // 애니메이션 시작
        
        yield return new WaitForSeconds(0.5f);
        GameObject instanceBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        Rigidbody bulletRb = instanceBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * 10;
        
        yield return new WaitForSeconds(2f);
        
        isChase = true;
        isAttack = false;
        // 애니메이션 끝

    }
}
