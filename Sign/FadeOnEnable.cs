using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeOnEnable : MonoBehaviour
{
    public float fadeDuration = 1f; // เวลา Fade
    public bool includeAlpha = false; // ถ้าอยากให้คงค่าโปร่งใสเดิมไว้
    public string ignoreTag = "backin"; // 🏷 Tag ที่จะไม่ให้ Fade

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Image> uiImages = new List<Image>();

    private void Awake()
    {
        // หา SpriteRenderer และ Image ทุกตัวในตัวเอง + ลูกทั้งหมด
        SpriteRenderer[] allSpriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        Image[] allImages = GetComponentsInChildren<Image>(true);

        // ✅ ข้าม GameObject ที่มี tag ที่กำหนด
        foreach (var sr in allSpriteRenderers)
        {
            if (sr == null) continue;
            if (sr.CompareTag(ignoreTag)) continue; // ถ้ามี tag นี้ → ข้าม
            spriteRenderers.Add(sr);
        }

        foreach (var img in allImages)
        {
            if (img == null) continue;
            if (img.CompareTag(ignoreTag)) continue; // ถ้ามี tag นี้ → ข้าม
            uiImages.Add(img);
        }

        // เริ่มต้นให้ทุกอันเป็นสีดำ
        foreach (var sr in spriteRenderers)
        {
            sr.color = Color.black;
        }

        foreach (var img in uiImages)
        {
            img.color = Color.black;
        }
    }

    private void OnEnable()
    {
        if(!GameObject.FindObjectOfType<JumpScareManager>().isJumpscare)
            StartCoroutine(FadeToWhiteCoroutine());
    }

    private IEnumerator FadeToWhiteCoroutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            foreach (var sr in spriteRenderers)
            {
                if (sr != null)
                {
                    Color start = Color.black;
                    Color end = Color.white;
                    if (includeAlpha) { start.a = sr.color.a; end.a = sr.color.a; }
                    sr.color = Color.Lerp(start, end, t);
                }
            }

            foreach (var img in uiImages)
            {
                if (img != null)
                {
                    Color start = Color.black;
                    Color end = Color.white;
                    if (includeAlpha) { start.a = img.color.a; end.a = img.color.a; }
                    img.color = Color.Lerp(start, end, t);
                }
            }

            yield return null;
        }

        // สุดท้ายให้เป็นสีขาวแน่นอน
        foreach (var sr in spriteRenderers)
        {
            if (sr != null)
                sr.color = Color.white;
        }

        foreach (var img in uiImages)
        {
            if (img != null)
                img.color = Color.white;
        }
    }
}
