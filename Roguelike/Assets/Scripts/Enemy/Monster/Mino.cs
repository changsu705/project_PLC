using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mino : Enemy
{
    public bool isLook;

    private Vector3 lookVec;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
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




    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 2);

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
                StartCoroutine(ShockWave());
                break;
        }

    }



    private IEnumerator Charging()
    {
        print("Charging");
        // 애니메이션 시작

        yield return new WaitForSeconds(0.1f);
        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        meleeArea.enabled = false;

        yield return new WaitForSeconds(2f);



        // 애니메이션 끝
        StartCoroutine(Think());
    }

    private IEnumerator ShockWave() // 기획 미정
    {
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(Think());
    }



    public override IEnumerator Attack()
    {

        // 애니메이션

        yield return new WaitForSeconds(0.2f); // 공격 로직 시작
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f); // 공격 로직 끝
        meleeArea.enabled = false;


        yield return new WaitForSeconds(1f); // 1초간 대기


        // 애니메이션 끝
        StartCoroutine(Think());
    }
}

