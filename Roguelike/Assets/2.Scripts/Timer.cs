using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private AnimationCurve curve;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.speed = 1f / time;
    }

    public void Playback()
    {
        var now = System.DateTime.Now;
        float hour = now.Hour * 30f + now.Minute * 0.5f;
        float minute = now.Minute * 6f;

        hourHand.localEulerAngles = new Vector3(0f, 0f, hour);
        minuteHand.localEulerAngles = new Vector3(0f, 0f, minute);

        StartCoroutine(ReturnTimer(minute, hour));
    }

    private IEnumerator ReturnTimer(float minute, float hour)
    {
        animator.Play("Timer");

        float currentTime = 0f;
        while (currentTime <= time)
        {
            hourHand.localEulerAngles = new Vector3(0f, 0f, hour - 360f * curve.Evaluate(currentTime / time));
            minuteHand.localEulerAngles = new Vector3(0f, 0f, minute - 4320f * curve.Evaluate(currentTime / time));
            currentTime += Time.deltaTime;

            yield return null;
        }
    }
}
