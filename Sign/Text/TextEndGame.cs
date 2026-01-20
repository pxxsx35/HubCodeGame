using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextEndGame : MonoBehaviour
{
    public string[] textEnd;       // ข้อความทั้งหมดที่จะโชว์
    public Image end;              // ภาพ Fade
    public float fadeDuration = 2f; // ระยะเวลา fade
    public float waitBeforeNext = 1f; // เวลาพักก่อนข้อความถัดไป

    private TypewriterEffectUI typewriter;

    void Start()
    {
        typewriter = FindObjectOfType<TypewriterEffectUI>();
        end.color = new Color(1, 1, 1, 0); // เริ่มจากโปร่งใส
        StartCoroutine(PlayEndingSequence());
    }

    private IEnumerator PlayEndingSequence()
    {
        // แสดงข้อความทั้งหมดตามลำดับ
        for (int i = 0; i < textEnd.Length; i++)
        {
            typewriter.ShowText(textEnd[i]);

            // ✅ รอจนกว่าข้อความจะพิมพ์จบ (คำนวณจากความยาวข้อความ × typingSpeed)
            float textDuration = textEnd[i].Length * typewriter.typingSpeed;
            yield return new WaitForSeconds(textDuration + waitBeforeNext);
        }

        // ✅ เมื่อจบข้อความทั้งหมด → Fade เป็นสีขาว
        yield return StartCoroutine(FadeToWhite());

        // ✅ รอ 2 วินาทีก่อนโหลด Scene ใหม่
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
    }

    private IEnumerator FadeToWhite()
    {
        float elapsed = 0f;
        Color color = end.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = alpha;
            end.color = color;
            yield return null;
        }
    }
}
