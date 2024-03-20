using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    public float targetRadius;
    public float targetRange;
    
    public Transform target;
    public BoxCollider meleeArea;

    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public bool isMino;
    
    
    protected Rigidbody rb;
    protected NavMeshAgent nav;
    
    
    private void Awake()
    {
        

        
        if(!isMino)
            Invoke(nameof(ChaseStart), 2f);
        
        
    }
    
    private void Update()
    {

        if (!isMino && nav.enabled) 
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }
    
    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

   
    /// <summary>
    /// 추적을 시작하는 로직
    /// </summary>
    private void ChaseStart()
    {
        isChase = true;
        // (애니 메이션 넣을 예정)
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 플레이어를 추적하는 로직
    /// </summary>
    public void Targeting()
    {
        if (!isMino && !isDead)
        {
            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward,
                targetRange,
                LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());

            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            currentHp -= 10;
            // 플레이어에게 데미지 받음
            // 일단 비어 둠
            // (애니메이션 넣을 예정)
            StartCoroutine(IsDead());
        }
    }

    private IEnumerator IsDead()
    {
        yield return new WaitForSeconds(0.1f);
        if (currentHp <= 0)
        {
            isDead = true;
            isChase = false;
            nav.enabled = false;
            //애니메이션
            Destroy(gameObject, 4f);
        }
    }

    /// <summary>
    /// 플레이어를 공격하는 로직
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator Attack();
    







}
