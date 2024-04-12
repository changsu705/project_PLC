using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private new PlayerAnimation animation;

    [Header("Player Stats")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    
    [SerializeField] private float speed;
    [SerializeField] private float dodgeForce = 5f;
    [SerializeField] private float dodgeCoolTime = 1f;
    private bool isDodge = false;
    private bool isDodgeCoolDown = true;
    private bool isDamage;

    [SerializeField] private SkillContainer[] skills;

    private float horizontal;
    private float vertical;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float sin;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float cos;

    /// <summary> base : 카메라 오일러 각 x </summary>
    private float tan;

    /// <summary> 45도 돌아간 움직임 </summary>
    public static readonly Quaternion quaterView = Quaternion.Euler(0f, 45f, 0f);

    private AudioManager audioManager;          //발소리 코드 추가
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 camAngle = Camera.main.transform.eulerAngles;

        sin = Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
        cos = Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
        tan = Mathf.Tan(camAngle.x * Mathf.Deg2Rad);

        audioManager = AudioManager.Instance;

        animation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        Vector3 screen2world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float y = screen2world.y - 1.5f;    //발이 아닌 눈이 마우스 바라보기
        float x = y / tan;

        screen2world.x += sin * x;
        screen2world.y = transform.position.y;
        screen2world.z += cos * x;

        transform.LookAt(screen2world, Vector3.up);

        if (!isDodge && (horizontal != 0f || vertical != 0f))
        {
            Vector3 lookNormal = (screen2world - transform.position).normalized;

            Vector3 movement = quaterView * new Vector3(horizontal, 0f, vertical);
            float dot = Vector3.Dot(lookNormal, movement);

            float totalSpeed = speed * (3f + dot) / 2f;
            transform.position += Time.deltaTime * totalSpeed * movement;

            animation.SetMovementValue(Vector3.Cross(lookNormal, movement).y, dot);

            audioManager.Footstep(true);
        }
        else
        {
            audioManager.Footstep(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDamage)
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
            
        }
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
            isDodge = true;
            isDodgeCoolDown = false;
            StartCoroutine(Dodge(transform.position + (quaterView * new Vector3(horizontal, 0f, vertical) * dodgeForce), 0.2f));
            StartCoroutine(DodgeCoolTime());
        }
    }

    private IEnumerator Dodge(Vector3 diredtion, float time)
    {
        Vector3 startPos = transform.position;

        float currentTime = 0f;
        while (currentTime < time)
        {
            transform.position = Vector3.Lerp(startPos, diredtion, currentTime / time);
            currentTime += Time.deltaTime;

            yield return null;
        }

        isDodge = false;
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
                IEnumerator routine = skills[idx].PlaySkill(this);
                if (routine != null)
                {
                    StartCoroutine(routine);
                }
            }
        }
    }
    #endregion
}
