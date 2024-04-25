using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Mino : Enemy
{
    [Header("Mino Stats")]
    public bool isLook;
    public GameObject ShorkWave;

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
        
        GameObject targetObject = GameObject.FindWithTag("Player");
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("플레이어 미아");
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
            ShorkWave.SetActive(false);
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
        yield return new WaitForSeconds(0.5f);

        int ranAction = Random.Range(0, 3);

        switch (ranAction)
        {
            case 0:
               
                // 점프 공격
                StartCoroutine(JumpAttack());
                
                break;
            case 1:
                // 달린 후 공격
                StartCoroutine(RunAttack());
                
                break;
            case 2:
                // 휘두르다
                StartCoroutine(Combo());
                
                
                break;
        }

    }



    private IEnumerator JumpAttack()
    {
        anim.SetTrigger("doShout");
        yield return new WaitForSeconds(2.3f);
        
        anim.SetTrigger("doJumpAttack");
        isLook = false;
        nav.isStopped = false;
        
        tauntVec = target.position + lookVec;
        
        
        
        yield return new WaitForSeconds(1.9f);
        ShorkWave.SetActive(true);
        
        yield return new WaitForSeconds(1.9f);
        ShorkWave.SetActive(false);
        isLook = true;
        nav.isStopped = true;

        StartCoroutine(Think());
    }

    private IEnumerator RunAttack()
    {

        anim.SetTrigger("doRun");
        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        
        
        yield return new WaitForSeconds(1f);
      
        
        anim.SetTrigger("doAttack");
        isLook = true;
        nav.isStopped = true;
        yield return new WaitForSeconds(0.8f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.5f);
        

        StartCoroutine(Think());
    }



    
    private IEnumerator Combo()
    {
        anim.SetTrigger("doWait");
        yield return new WaitForSeconds(2.3f);
        
        anim.SetTrigger("doCombo");
       
        yield return new WaitForSeconds(0.7f);
        
        meleeArea.enabled = true;
        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;
        
        yield return new WaitForSeconds(1f);
        ShorkWave.SetActive(true);
        yield return new WaitForSeconds(1f);
        ShorkWave.SetActive(false);
        yield return new WaitForSeconds(2f);
        
        
        StartCoroutine(Think());
    }
    public override IEnumerator Attack()
    {
       yield return null;
    }
}

