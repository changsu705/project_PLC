using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dashScale;
    [SerializeField] private float dodgeForce = 5f;
    [SerializeField] private float dodgeCoolTime = 1f;
    private bool isDodge = false;

    [Header("Battle Stat")]
    [SerializeField] private float hp;
    [SerializeField] private float atk;
    [SerializeField] private float[] attackCoolTimes;
    [SerializeField] private float[] hitBoxActiveTimes;
    private readonly bool[] isAttack = { false, false, false, false, false, false, };

    [Header("Skill Colliders")]
    [SerializeField] private GameObject[] skillColliders;

    private float horizontal;
    private float vertical;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float sin;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float cos;

    /// <summary> base : 카메라 오일러 각 x </summary>
    private float tan;

    /// <summary> 45도 돌아간 움직임 </summary>
    private readonly Quaternion quaterView = Quaternion.Euler(0f, 45f, 0f);

    private void Start()
    {
        Vector3 camAngle = Camera.main.transform.eulerAngles;

        sin = Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
        cos = Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
        tan = Mathf.Tan(camAngle.x * Mathf.Deg2Rad);
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

        if (horizontal != 0f || vertical != 0f)
        {
            Vector3 movement = quaterView * new Vector3(horizontal, 0f, vertical);

            float totalSpeed = speed * (3f + Vector3.Dot((screen2world - transform.position).normalized, movement)) / 2f;

            transform.position += Time.deltaTime * totalSpeed * movement;
        }
    }

    #region InputSystem
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();

        horizontal = v.x;
        vertical = v.y;
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isDodge && (horizontal != 0f || vertical != 0f))
        {
            transform.position += quaterView * new Vector3(horizontal, 0f, vertical) * dodgeForce;
            isDodge = true;
            StartCoroutine(DodgeCoolTime());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (context.ReadValue<float>())
            {
                case 0f:
                    if (!isAttack[0])
                    {
                        Vector3 pos = transform.position;
                        pos.y += 1f;
                        SkillEffects.Instance.PlayEffect(SkillEffects.FX.BasicSmash, pos, transform.rotation);

                        skillColliders[0].transform.SetPositionAndRotation(pos, transform.rotation);
                        skillColliders[0].SetActive(true);

                        StartCoroutine(AttackHitBoxDisable(0));
                        StartCoroutine(AttackCoolTime(0));
                    }
                    break;

                case 1f:
                    if (!isAttack[1])
                    {
                        Vector3 pos = transform.position;
                        pos.y += 1f;
                        SkillEffects.Instance.PlayEffect(SkillEffects.FX.FireBall, pos, transform.rotation);

                        skillColliders[1].transform.SetPositionAndRotation(pos, transform.rotation);
                        skillColliders[1].SetActive(true);

                        StartCoroutine(AttackHitBoxDisable(1));
                        StartCoroutine(AttackCoolTime(1));
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private IEnumerator DodgeCoolTime()
    {
        yield return new WaitForSeconds(dodgeCoolTime);
        isDodge = false;
    }

    private IEnumerator AttackHitBoxDisable(int attackIdx)
    {
        yield return new WaitForSeconds(hitBoxActiveTimes[attackIdx]);
        skillColliders[attackIdx].SetActive(false);
    }

    private IEnumerator AttackCoolTime(int attackIdx)
    {
        isAttack[attackIdx] = true;
        yield return new WaitForSeconds(attackCoolTimes[attackIdx]);
        isAttack[attackIdx] = false;
    }
    #endregion
}
