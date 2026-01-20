using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    private Image currentImg;
    private Image currentBgImg;
    public bool isPopupOpen = false;
    private bool ignoreNextClick = false;
    public TypewriterEffectUI typewriter; // ลากสคริปต์ TypewriterEffectUI มาใน Inspector

    void Update()
    {
        if (!isPopupOpen) return;

        if (ignoreNextClick)
        {
            // ข้ามคลิกใน frame ถัดไป
            ignoreNextClick = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (typewriter != null && typewriter.IsTyping())
            {
                // ขณะพิมพ์ → Skip
                typewriter.SkipText();
            }
            else
            {
                // ขณะพิมพ์ครบ → ปิด Popup
                ClosePopup();
            }
        }
    }

    public void OpenPopup(Image img, Image bgImg)
    {
        currentImg = img;
        currentBgImg = bgImg;

        currentImg.rectTransform.pivot = new Vector2(0.5f, 0.65f);
        currentBgImg.rectTransform.pivot = new Vector2(0.5f, 0.65f);

        isPopupOpen = true;
        ignoreNextClick = true; // ข้ามคลิก frame นี้
    }

    public void ClosePopup()
    {
        if (currentImg != null && currentBgImg != null)
        {
            currentImg.rectTransform.pivot = new Vector2(2, 2);
            currentBgImg.rectTransform.pivot = new Vector2(2, 2);

            // ลบข้อความหลังปิด Popup
            if (typewriter != null)
            {
                typewriter.StopTypingImmediate(); // ฟังก์ชันลบข้อความทันที
            }
        }

        isPopupOpen = false;
    }
}
