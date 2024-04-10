using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private readonly int X = Animator.StringToHash("X");
    private readonly int Y = Animator.StringToHash("Y");

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    public void SetMovementValue(float x, float y)
    {
        animator.SetFloat(X, x);
        animator.SetFloat(Y, y);
    }
}
