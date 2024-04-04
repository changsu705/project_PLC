using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dodgeForce = 5f;
    [SerializeField] private float dodgeCoolTime = 1f;
    private bool isDodge = false;
    private bool isDodgeCoolDown = true;

    [SerializeField] private SkillContainer[] skills;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float sin;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float cos;

    /// <summary> base : 카메라 오일러 각 x </summary>
    private float tan;

    /// <summary> 45도 돌아간 움직임 </summary>
    public static readonly Quaternion quaterView = Quaternion.Euler(0f, 45f, 0f);

    private AudioManager audioManager;          //발소리 코드 추가

    private void Start()
    {
        Vector3 camAngle = Camera.main.transform.eulerAngles;

        sin = Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
        cos = Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
        tan = Mathf.Tan(camAngle.x * Mathf.Deg2Rad);

        audioManager = AudioManager.instance;
    }

    private bool isMoving = false; // 플레이어의 움직임 상태를 추적하는 변수

    private IEnumerator DelayedFootstep()         //발소리 코드 추가
    {
        yield return new WaitForSeconds(0.1f);
        //AudioManager.instance.PlayFootstep("Footstep1");
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

        if (!isDodge && (Horizontal != 0f || Vertical != 0f))
        {
            Vector3 movement = quaterView * new Vector3(Horizontal, 0f, Vertical);

            float totalSpeed = speed * (3f + Vector3.Dot((screen2world - transform.position).normalized, movement)) / 2f;

            transform.position += Time.deltaTime * totalSpeed * movement;

            if (!isMoving)          //발소리 코드 추가
            {
                isMoving = true;
                StartCoroutine(DelayedFootstep());
            }
        }
        else
        {
            //AudioManager.instance.StopFootstep();
            isMoving = false;
        }

     }

    #region InputSystem
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();

        Horizontal = v.x;
        Vertical = v.y;
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isDodgeCoolDown && (Horizontal != 0f || Vertical != 0f))
        {
            isDodge = true;
            isDodgeCoolDown = false;
            StartCoroutine(Dodge(transform.position + (quaterView * new Vector3(Horizontal, 0f, Vertical) * dodgeForce), 0.2f));
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
                StartCoroutine(skills[idx].PlaySkill(this));
            }
        }
    }
    #endregion
}
