using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    public float fadeDuration = 1f; // 페이드 인/아웃에 걸리는 시간

    private bool canLoadScene = false;
    private bool isLoadingScene = false;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = FindObjectOfType<CanvasGroup>(); // 씬에 있는 CanvasGroup 컴포넌트를 찾음
        canvasGroup.alpha = 0f; // 초기에는 투명
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌했을 때
        {
            Debug.Log("player");
            canLoadScene = true; // 씬을 로드할 수 있도록 플래그 설정
        }
    }

    private void Update()
    {
        if (canLoadScene && !isLoadingScene)
        {
            isLoadingScene = true;
            StartCoroutine(LoadSceneWithFade());
        }
    }

    private IEnumerator LoadSceneWithFade()
    {
        Scene nowScene = SceneManager.GetActiveScene();
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration); // 페이드 인
            yield return null;
        }

        switch (nowScene.buildIndex)
        {
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                SceneManager.LoadScene(4);
                break;
            case 4:
                SceneManager.LoadScene(5);
                break;
            case 5:
                SceneManager.LoadScene(6);
                break;
            case 6:
                SceneManager.LoadScene(7);
                break;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOutAfterSceneLoaded());
    }

    private IEnumerator FadeOutAfterSceneLoaded()
    {

        canvasGroup = FindObjectOfType<CanvasGroup>(); // 새로운 씬에서 CanvasGroup을 찾음

        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration); // 페이드 아웃
            yield return null;
        }

        isLoadingScene = false;
    }
}