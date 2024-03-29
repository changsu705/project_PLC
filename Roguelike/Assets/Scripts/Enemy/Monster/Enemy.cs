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
        anim.SetBool("isRun", true);
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
        if (!isDead && other.CompareTag("Weapon")) 
        {
            currentHp -= 10;
            SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicHit, transform.position, Quaternion.identity);
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
            
            // 주인공의 공격을 맞았을 때 애니메이션
            // (애니메이션 넣을 예정)
        }
    }

    private IEnumerator OnDamage(Vector3 reactVec)
    {
        foreach (MeshRenderer mesh in renderers)
        {
            mesh.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.1f);

        if (currentHp > 0)
        {
            reactVec = reactVec.normalized;
            reactVec+= Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
            
            foreach (var pair in originalColors)
            {
                pair.Key.material.color = pair.Value;
            }
        }
        
        else
        {
            foreach (MeshRenderer mesh in renderers)
            {
                mesh.material.color = Color.gray;
            }
            
            gameObject.layer = 0;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            reactVec = reactVec.normalized;
            reactVec+= Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject,2f);

        }
    }

    

    /// <summary>
    /// 플레이어를 공격하는 로직
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator Attack();
}
