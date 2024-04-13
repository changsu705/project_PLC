using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private readonly int xHash = Animator.StringToHash("X");
    private readonly int yHash = Animator.StringToHash("Y");
    private readonly int aniValueHash = Animator.StringToHash("AniValue");
    private readonly int playHash = Animator.StringToHash("PlayAni");

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    public void SetMovementValue(float x, float y)
    {
        animator.SetFloat(xHash, x);
        animator.SetFloat(yHash, y);
    }

    public void PlaySkillAnimation(int value)
    {
        animator.SetInteger(aniValueHash, value);
        animator.SetTrigger(playHash);
    }
}
