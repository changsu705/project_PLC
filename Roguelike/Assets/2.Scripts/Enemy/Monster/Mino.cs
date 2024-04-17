using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mino : Enemy
{
    
    
    public bool isLook;

    private Vector3 lookVec;
    private Vector3 tauntVec;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        
        foreach (SkinnedMeshRenderer mesh in renderers)
        {
            originalColors[mesh] = mesh.material.color;
        }
        
        nav.isStopped = true;
    }

    private void Start()
    {
        StartCoroutine(Think());
    }

    private void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v);
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);
        }
        

    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 2);

        switch (ranAction)
        {
            case 0:
               
                // 충격파
                StartCoroutine(Taunt());
                
                break;
            case 1:
                // 돌진
                StartCoroutine(Charging());
                
                break;
            case 2:
                // 휘두르다
                StartCoroutine(Attack());
                
                
                break;
        }

    }



    private IEnumerator Charging()
    {
        anim.SetTrigger("doCharge");

        yield return new WaitForSeconds(0.5f);
        // rb.AddForce(transform.forward * 10, ForceMode.Impulse);
        
        
        
        
        
        anim.SetTrigger("doAttack");
        meleeArea.enabled = true;
        

        yield return new WaitForSeconds(2.1f);
        meleeArea.enabled = false;
       

        StartCoroutine(Think());
    }

    private IEnumerator Taunt()
    {

        anim.SetTrigger("doSound");
        yield return new WaitForSeconds(2.5f);
        
        // tauntVec = target.position + lookVec;
        // isLook = false;
        // nav.isStopped = false;
        anim.SetTrigger("doTaunt");
        
       
        
        
       
        yield return new WaitForSeconds(3.1f);
        
        //isLook = true;
        //nav.isStopped = true;
        
        anim.SetTrigger("doAttack");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(2.1f);
        meleeArea.enabled = false;
       

        StartCoroutine(Think());
    }



    public override IEnumerator Attack()
    {
        anim.SetTrigger("doAttack");
        print("Attack");
        

        yield return new WaitForSeconds(0.2f); // 공격 로직 시작
        //meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f); // 공격 로직 끝
        //meleeArea.enabled = false;


        yield return new WaitForSeconds(1f); // 1초간 대기


        
        StartCoroutine(Think());
    }
}

