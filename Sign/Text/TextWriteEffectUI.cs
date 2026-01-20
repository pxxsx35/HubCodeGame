using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffectUI : MonoBehaviour
{
    public Text textComponent;
    public float typingSpeed = 0.05f;
    public AudioSource audioSource;      // ใส่ AudioSource ใน Inspector
    public AudioClip typeSound;          // เสียงที่เล่นระหว่างพิมพ์

    private Coroutine typingCoroutine;
    private string currentFullText;
    private bool isTyping = false;
    private bool textFullyShown = false;

    public void ShowText(string fullText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentFullText = fullText;
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        textFullyShown = false;
        textComponent.text = "";
        if (audioSource != null && typeSound != null)
        {
            audioSource.clip = typeSound;
            audioSource.Play();
        }
        foreach (char c in currentFullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (audioSource != null)
            audioSource.Stop();

        isTyping = false;
        textFullyShown = true;
    }

    public void SkipText()
    {
        if (!isTyping) return;

        StopCoroutine(typingCoroutine);
        textComponent.text = currentFullText;
        isTyping = false;
        textFullyShown = true;

        if (audioSource != null)
            audioSource.Stop();
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    public void StopTypingImmediate()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = null;
        textComponent.text = "";
        isTyping = false;
        textFullyShown = false;
    }
}
