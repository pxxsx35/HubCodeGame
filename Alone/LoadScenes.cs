using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoasScenes : MonoBehaviour
{
    public Image fadeImage; // UI Image ที่ใช้ Fade
    public AudioSource sound;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string Test)
    {
        sound.Play();

        StartCoroutine(FadeOut(Test));
    }

    IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        for (float t = 1f; t > 0; t -= Time.deltaTime)
        {
            color.a = t;
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(string Test)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        for (float t = 0f; t < 1; t += Time.deltaTime)
        {
            color.a = t;
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(Test);
    }
}
