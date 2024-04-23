using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static GameManager;

public class PlayerController : MonoBehaviour
{
    private new PlayerAnimation animation;

    [Header("Player Stats")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    
    [SerializeField] private float speed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeCoolTime;
    [SerializeField] private float attackRange;     //오브젝트 파괴하는데 씀


    private float rotationScale = 1f;
    private bool isDodge = false;
    private bool isDodgeCoolDown = true;
    private bool isDamage;
    private bool isDie = false;
    
    public Image hpBar;

    [SerializeField] private SkillObject[] skills;

    private float horizontal;
    private float vertical;

    /// <summary> 돌아간 카메라에 따른 움직임 </summary>
    public static Quaternion QuaterView => Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
    private Plane plane;

    private AudioManager audioManager;          //발소리 코드 추가
    private Rigidbody rb;

    private new CapsuleCollider collider;
    private NavMeshAgent navMeshAgent;

    [Header("Audios")]
    [SerializeField] private AudioClip[] footstepClips;
    private const int Grass = 1 << 4;
    private const int Stone = 1 << 7;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitBarSize();
    }

    private void Start()
    {
        plane = new Plane(transform.up, transform.position);

        audioManager = AudioManager.Instance;

        collider = GetComponent<CapsuleCollider>();
        animation = GetComponent<PlayerAnimation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        // 플레이어 주변의 파괴 가능한 오브젝트를 찾아서 파괴
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Weeds") || col.CompareTag("Totem"))
            {
                GameObject destroyedObject = col.gameObject;
                Destroy(destroyedObject);
                ObjectDestroyedEvent.InvokeObjectDestroyed(destroyedObject);
            }
        }
    }

    private void Update()
    {
        if (!isDie && !isDodge)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            plane.Raycast(ray, out float enter);

            Vector3 hitPoint = ray.GetPoint(enter);
            hitPoint.y = transform.position.y;

            transform.LookAt(hitPoint);

            //이동 벡터와 시선 벡터의 각에 따라 다른 모션과 속도
            if (!isDodge && (horizontal != 0f || vertical != 0f))
            {
                Vector3 lookNormal = (hitPoint - transform.position).normalized;

                Vector3 movement = QuaterView * new Vector3(horizontal, 0f, vertical);
                float dot = Vector3.Dot(lookNormal, movement);

                float totalSpeed = speed * (3f + dot) / 2f;
                transform.position += Time.deltaTime * totalSpeed * movement;

                animation.SetMovementValue(Vector3.Cross(lookNormal, movement).y, dot);

                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    switch (hit.mask)
                    {
                        case Grass:
                            audioManager.FootstepPlayer.clip = footstepClips[0];
                            break;

                        case Stone:
                            audioManager.FootstepPlayer.clip = footstepClips[1];
                            break;

                        default:
                            break;
                    }
                }

                audioManager.Footstep(true);
            }
            else
            {
                audioManager.Footstep(false);
            }
        }
    }

    private void FixedUpdate()
    {
        //FreezeVelocity();
    }

    private void FreezeVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    
    private void InitBarSize()
    {
        hpBar.rectTransform.localScale = new Vector3(1, 1, 1);
    }
    
    private void UpdateHpBar()
    {
        float hpRatio = Mathf.Clamp01(currentHp / (float)maxHp);
        hpBar.rectTransform.localScale= new Vector3(currentHp / (float)maxHp, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDie && !isDamage)
        {
            if (other.CompareTag("EnemyBullet"))
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                StartCoroutine(OnDamage(enemyBullet.damage));
                
            }
        }
    }
    
    private IEnumerator OnDamage(float damage)
    {
        // isDamage 을 이용하여 중복 데미지를 방지
        isDamage = true;
        currentHp -= damage;
        
        if (currentHp <= 0)
        {
            currentHp = 0;
            isDie = true;

            animation.PlayerDie();
            GameManager.Instance.GameEnd();
        }
        UpdateHpBar();
        yield return new WaitForSeconds(1f);
        isDamage = false;
    }

    #region InputSystem
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();

        horizontal = v.x;
        vertical = v.y;

        if (horizontal == 0f && vertical == 0f)
        {
            animation.SetMovementValue(0f, 0f);
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isDodgeCoolDown && (horizontal != 0f || vertical != 0f))
        {
            rotationScale = 0f;
            isDodge = true;
            isDodgeCoolDown = false;

            collider.enabled = false;

            animation.Dodge();
            audioManager.Footstep(false);

            StartCoroutine(Dodge(transform.position + (QuaterView * new Vector3(horizontal, 0f, vertical) * dodgeForce), 0.2f));
            StartCoroutine(DodgeCoolTime());
        }
    }

    private IEnumerator Dodge(Vector3 endPos, float time)
    {
        transform.LookAt(endPos);
        Vector3 startPos = transform.position;

        //dodge
        float currentTime = 0f;
        while (currentTime < time)
        {
            transform.position += (endPos - startPos) * Time.deltaTime / time;
            currentTime += Time.deltaTime;

            yield return null;
        }

        var (newStartPos, newEndPos) = (endPos, (endPos - startPos).normalized * 1.5f + endPos);

        //roll
        currentTime = 0f;
        while (currentTime < 0.5f)
        {
            transform.position += (newEndPos - newStartPos) * Time.deltaTime * 2f;
            currentTime += Time.deltaTime;

            yield return null;
        }

        isDodge = false;
        collider.enabled = true;
        
        //구르기 끝났을 때 마우스 부드럽게 바라보기
        while (rotationScale < 1f)
        {
            rotationScale += Time.deltaTime * 3f;

            yield return null;
        }
    }

    private IEnumerator DodgeCoolTime()
    {
        yield return new WaitForSeconds(dodgeCoolTime);
        isDodgeCoolDown = true;
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            int idx = (int)context.ReadValue<float>();
            if (idx < skills.Length && skills[idx].AttackCoolDown)
            {
                StartCoroutine(skills[idx].PlaySkill(this));
                animation.PlaySkillAnimation(skills[idx].CurrentContainer.AnimationKey);
                audioManager.PlaySFX(skills[idx].CurrentContainer.StartClip);
            }
        }
    }
    #endregion
}
