using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour 
{
    [Header("Enemy Stats")]
    public float maxHp;
    public float currentHp;
    
    public float targetRadius;
    public float targetRange;
    
    [Space(10)]
    
    [Header("Enemy Components")]
    public Transform target;
    public Collider meleeArea;
    public Image hpBar;
    public GameObject hudDamageText;
    public Transform hudPos;
    
    [Space(10)]

    [Header("Enemy Flags")]
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public bool isMino;
    public bool isDamage;
    public bool isStart;
    public bool isUpgraded;

    [Space(10)] 
    [Header("DissolvingController")]

    [Space(10)]
    
    protected Rigidbody rb;
    protected NavMeshAgent nav;
    protected Animator anim;
    
    [Header("Enemy Renderers")]
    public Material dissolveMaterial;
    public SkinnedMeshRenderer[] renderers;
    protected Dictionary<SkinnedMeshRenderer,Color> originalColors = new Dictionary<SkinnedMeshRenderer, Color>();
    


    private void Awake()
    {
        GameObject targetObject = GameObject.FindWithTag("Player");
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("플레이어 미아");
        }
        
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        dissolveMaterial = new Material(dissolveMaterial);
        
        foreach (SkinnedMeshRenderer mesh in renderers)
        {
            originalColors[mesh] = mesh.material.color;
        }

        //InitBarSize();

    }

    private void Start()
    {
        // if (!isMino)
        // {
        //     Invoke(nameof(ChaseStart), 2f);
        // }
    }

    private void Update()
    {
        CheckMaxHp();
        
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
    
        if (distanceToPlayer < 12.5f && !isMino && !isStart) 
        {
            ChaseStart();
            isStart = true;
        }
        
        Debug.DrawRay(transform.position, transform.forward * targetRange, Color.red);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

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

    // private void InitBarSize()
    // {
    //     hpBar.rectTransform.localScale = new Vector3(1, 1, 1);
    // }


    /// <summary>
    /// 추적을 시작하는 로직
    /// </summary>
    private void ChaseStart()
    {
        anim.SetBool("isWalk", true);
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

    private void CheckMaxHp()
    {
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        UpdateHpBar();

    }

    /// <summary>
    /// 플레이어를 추적하는 로직
    /// </summary>
    private void Targeting()
    {
        if (!isMino && !isDead && !isDamage && !isAttack) 
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

        if (!isDead && other.CompareTag("Skill") && !isDamage ) 
        {
            var container = other.GetComponent<SkillControl>();
            currentHp -= container.Container.ATK;
            if (currentHp < 0)
            {
                currentHp = 0;
            }
            
            UpdateHpBar();
            
            //SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicHit, transform.position, Quaternion.identity);
            Vector3 reactVec = transform.position - other.transform.position;
            
            GameObject hudText = Instantiate(hudDamageText);    
            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = container.Container.ATK;
            
            isChase= false;
            isAttack = false;
            StartCoroutine(OnDamage(reactVec));
        }
        
        
    }

    private void UpdateHpBar()
    {
        float hpRatio = Mathf.Clamp01(currentHp / (float)maxHp);
        hpBar.rectTransform.localScale= new Vector3(currentHp / (float)maxHp, 1, 1);
    }

    private IEnumerator OnDamage(Vector3 reactVec)
    {
        
        if (currentHp > 0 ) 
        {
            anim.SetTrigger("doDamage");
            isDamage = true;
            isChase = false;
            isAttack = false;

            foreach (SkinnedMeshRenderer mesh in renderers)
            {
                mesh.material.color = Color.red;
            }
            
            yield return new WaitForSeconds(0.1f);
          
            isDamage= false;
            
            foreach (var pair in originalColors)
            {
                pair.Key.material.color = pair.Value;
            }
            
            
            
            yield return new WaitForSeconds(1f);
            
            
            if (!isMino)
            {
                isChase = true;
                anim.SetBool("isWalk", true);
            }

        }
        
        
        else
        {
            StopAllCoroutines();
            anim.SetTrigger("doDie");
            isDead = true;
            nav.enabled = false;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
            gameObject.layer = 0;
            rb.isKinematic = true;
            
            if (!isMino)
            {
                StartCoroutine(Dissolve());
                isChase = false;
                
                Destroy(gameObject,1f);
            }
            

            

        }
    }

    public IEnumerator Dissolve()
    {
        foreach (SkinnedMeshRenderer meshRenderer in renderers)
        {
            Material[] materials = meshRenderer.materials;
        
            for(int index = 0; index < materials.Length; index++)
            {
                materials[index] = dissolveMaterial;
            }
        
            meshRenderer.materials = materials;
        }
    
        dissolveMaterial.DOFloat(1, "_Float", 2);
    
        yield return null;
    }




    /// <summary>
    /// 플레이어를 공격하는 로직
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator Attack();
}
