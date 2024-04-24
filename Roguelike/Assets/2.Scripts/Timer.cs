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
        DontDestroyOnLoad(gameObject);

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

        StartCoroutine(ReturnHome());
        StartCoroutine(ReturnTimer(minute, hour));
    }

    private IEnumerator ReturnHome()
    {
        float halfTime = time / 2f;
        float currentTime = 0f;
        while (currentTime < halfTime)
        {
            Time.timeScale = 1f - currentTime / halfTime;
            currentTime += Time.unscaledDeltaTime;

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);

        currentTime = 0f;
        while (currentTime < halfTime)
        {
            Time.timeScale = currentTime / halfTime;
            currentTime += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 1f;
    }

    private IEnumerator ReturnTimer(float minute, float hour)
    {
        animator.Play("Timer");

        float currentTime = 0f;
        while (currentTime <= time)
        {
            hourHand.localEulerAngles = new Vector3(0f, 0f, hour - 360f * curve.Evaluate(currentTime / time));
            minuteHand.localEulerAngles = new Vector3(0f, 0f, minute - 4320f * curve.Evaluate(currentTime / time));
            currentTime += Time.unscaledDeltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
