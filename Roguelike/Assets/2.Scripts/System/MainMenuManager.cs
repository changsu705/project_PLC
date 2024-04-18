using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    private bool isFading = false;

    public void StartGame()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        while (fadeImage.color.a < 1)
        {
            color.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene("HouseScene");
    }
}