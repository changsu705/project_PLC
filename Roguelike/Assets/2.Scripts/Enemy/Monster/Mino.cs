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
            
                anim.SetBool("isLeft", true);
            
            
            
        }
        else
        {
            nav.SetDestination(tauntVec);
        }




    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 3);

        switch (ranAction)
        {
            case 0:
                // 휘두르다
                StartCoroutine(Attack());
                break;
            case 1:
                // 돌진
                StartCoroutine(Charging());
                break;
            case 2:
                // 충격파
                StartCoroutine(Taunt());
                break;
        }

    }



    private IEnumerator Charging()
    {
        print("Charging");
        anim.SetTrigger("doCharge");

        yield return new WaitForSeconds(0.1f);
        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
        
        //meleeArea.enabled = true;
        

        yield return new WaitForSeconds(0.5f);
        
        rb.velocity = Vector3.zero;
        //meleeArea.enabled = false;

        yield return new WaitForSeconds(2f);
        

        // 애니메이션 끝
        StartCoroutine(Think());
    }

    private IEnumerator Taunt() 
    {
        anim.SetTrigger("doTaunt");
        print("Taunt");
        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        
        yield return new WaitForSeconds(0.5f);
        
        isLook = true;
        nav.isStopped = true;
       

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
