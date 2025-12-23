using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PasswordFeedback : MonoBehaviour
{
    public Image correctImage;  // รูปเมื่อใส่รหัสผ่านถูก
    public Image incorrectImage; // รูปเมื่อใส่รหัสผ่านผิด
    public float displayDuration = 1.5f; // เวลาที่จะแสดงรูป

    void Start()
    {
        // ซ่อนภาพเมื่อเริ่มเกม
        HideFeedback();
    }

    public void ShowCorrect()
    {
        correctImage.gameObject.SetActive(true);
        incorrectImage.gameObject.SetActive(false);
        StartCoroutine(HideAfterDelay());
    }

    public void ShowIncorrect()
    {
        incorrectImage.gameObject.SetActive(true);
        correctImage.gameObject.SetActive(false);
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        HideFeedback();
    }

    private void HideFeedback()
    {
        correctImage.gameObject.SetActive(false);
        incorrectImage.gameObject.SetActive(false);
    }
}
