using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [Header("ภาพตอนจบตามลำดับ (Image)")]
    public Image[] scenes; // แทน Scenceone, Scencetwo, Scencethree

    [Header("ตั้งค่าการ Fade")]
    public float fadeDuration = 1f;  // ความเร็วในการ fade
    public float holdDuration = 2f;  // ค้างไว้ก่อน fade ออก

    private MouseTexture mouse;

    private void Start()
    {
        // ปิดภาพทั้งหมดก่อนเริ่ม
        foreach (var img in scenes)
        {
            if (img != null)
                img.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        mouse = FindObjectOfType<MouseTexture>();
        Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
        SceneManager.LoadScene("Lose");
        Debug.Log("Bad End");
    }

    public void BadEnd()
    {
        StartCoroutine(PlayEndScenes());
    }

    private IEnumerator PlayEndScenes()
    {
        // ปิด UI อื่น ๆ ถ้ามี
      

        var room = GameObject.Find("room");
        if (room) room.SetActive(false);

        // แสดงภาพแต่ละอันตามลำดับ
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i] != null)
                yield return StartCoroutine(FadeSequence(scenes[i]));
        }

        // 🎬 ครบทุกภาพแล้ว → โหลด Scene Main
        SceneManager.LoadScene("Main");
    }

    private IEnumerator FadeSequence(Image img)
    {
        img.gameObject.SetActive(true);
        img.enabled = true;

        Color c = img.color;
        c.a = 0f;
        img.color = c;

        float t = 0f;

        // 🟢 Fade In
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            img.color = c;
            yield return null;
        }
        c.a = 1f;
        img.color = c;

        // 🕒 ค้างไว้
        yield return new WaitForSeconds(holdDuration);

        // 🔴 Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            img.color = c;
            yield return null;
        }
        c.a = 0f;
        img.color = c;

        img.enabled = false;
    }

}
