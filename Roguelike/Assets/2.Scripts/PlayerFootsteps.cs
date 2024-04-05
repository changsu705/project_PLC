using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip footstepSound; // 발걸음 소리 파일
    public float stepInterval = 0.5f; // 발걸음 간격
    private float stepTimer = 0f;

    private AudioSource audioSource; // AudioSource 컴포넌트 참조
    private Rigidbody rb; // Rigidbody 컴포넌트 참조

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트 추가
    }

    private void Update()
    {
        // 플레이어가 움직일 때만 발걸음 소리 재생
        if (Mathf.Abs(rb.velocity.magnitude) > 0.08f && IsGrounded())
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;
                if (!audioSource.isPlaying) // 소리가 재생 중이 아닌 경우에만 재생
                {
                    PlayFootstepSound();
                }
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private bool IsGrounded()
    {
        // 플레이어가 땅에 닿아 있는지 여부를 체크하는 코드
        // 여기서는 간단하게 RaycastHit을 사용하여 땅과의 거리를 측정합니다.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            return true;
        }
        return false;
    }

    private void PlayFootstepSound()
    {
        // 발걸음 소리 재생
        if (footstepSound != null)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }
}