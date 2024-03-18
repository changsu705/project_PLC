using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    
    public EnemyStat enemyStat;
    public Transform target;
    public BoxCollider meleeArea;

    public bool isChase;
    public bool isAttack;
    public bool isDead;
    
    protected Rigidbody rb;
    protected NavMeshAgent nav;
    
    
    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        nav=GetComponent<NavMeshAgent>();

        Invoke(nameof(ChaseStart), 2f);
    }
    
    private void Update()
    {
        if (nav.enabled)
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
    public abstract void Targeting();

    /// <summary>
    /// 플레이어를 공격하는 로직
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator Attack();
    







}
