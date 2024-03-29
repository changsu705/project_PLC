using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


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
    
    protected Dictionary<MeshRenderer,Color> originalColors = new Dictionary<MeshRenderer, Color>();


    protected Rigidbody rb;
    protected NavMeshAgent nav;
    protected Animator anim;
    public MeshRenderer[] renderers;
    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        
        foreach (MeshRenderer mesh in renderers)
        {
            originalColors[mesh] = mesh.material.color;
        }

    }

    private void Start()
    {
        if (!isMino)
        {
            Invoke(nameof(ChaseStart), 2f);
        }
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
        // anim.SetBool("isWalk", true);
        isChase = true;

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
    private void Targeting()
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
            SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicHit, transform.position, Quaternion.identity);
            StartCoroutine(OnDamage());
            currentHp -= 10;
            // 일단 비어 둠
            // (애니메이션 넣을 예정)
        }
    }

    private IEnumerator OnDamage()
    {
       
    
        foreach (MeshRenderer mesh in renderers)
        {
            mesh.material.color = Color.red;
        }


        if (!isMino)
        {
            nav.enabled = false;
            Vector3 backVec = -transform.forward * 20f;
            rb.AddForce(backVec, ForceMode.Impulse);
            yield return new WaitForSeconds(0.2f);
            nav.enabled = true;
            print("넉백");
        }
        

        if (currentHp <= 0)
        {
            isDead = true;
            isChase = true;
            nav.enabled = false;
            gameObject.layer = 0;
            //anim.SetTrigger("doDie");

            yield return new WaitForSeconds(0.1f);

            foreach (MeshRenderer mesh in renderers)
            {
                mesh.material.color = Color.gray;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        
            // foreach(MeshRenderer mesh in renderers)
            // {
            //     mesh.material.color = Color.white;
            // }
            
            foreach (var pair in originalColors)
            {
                 pair.Key.material.color = pair.Value;
            }
        }
    }

    /// <summary>
    /// 플레이어를 공격하는 로직
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator Attack();
}
