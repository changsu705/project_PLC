using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dashScale;

    [Header("Battle Stat")]
    [SerializeField] private float hp;
    [SerializeField] private float damage;
    [SerializeField] private float[] attackCoolTimes = { 1f, 1f, 1f };

    private readonly bool[] canAttack = { true, true, true };

    private float horizontal;
    private float vertical;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float sin;

    /// <summary> base : 카메라 오일러 각 y </summary>
    private float cos;

    /// <summary> base : 카메라 오일러 각 x </summary>
    private float tan;

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
            Vector3 movement = Quaternion.Euler(0f, 45f, 0f) * new Vector3(horizontal, 0f, vertical);

            float totalSpeed = speed * (3f + Vector3.Dot((screen2world - transform.position).normalized, movement)) / 2f;

            transform.position += Time.deltaTime * totalSpeed * movement;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();

        horizontal = v.x;
        vertical = v.y;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                speed *= dashScale;
                break;

            case InputActionPhase.Canceled:
                speed /= dashScale;
                break;

            default:
                break;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (context.ReadValue<float>())
            {
                case 0f:
                    if (canAttack[0])
                    {
                        canAttack[0] = false;
                        StartCoroutine(AttackCoolTime(0));
                        Debug.Log("Basic");
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private IEnumerator AttackCoolTime(int attackIdx)
    {
        yield return new WaitForSeconds(attackCoolTimes[attackIdx]);
        canAttack[attackIdx] = true;
        Debug.Log("Attack!");
    }
}
