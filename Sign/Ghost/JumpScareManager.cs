using System.Collections;
using UnityEngine;

public class JumpScareManager : MonoBehaviour
{
    public Camera mainCamera;          // กล้องหลัก
    public float shakeIntensity = 0.2f;
    public float shakeDuration = 2f;
    public SpriteRenderer[] room;
    public GameObject allUI;
    public bool isJumpscare;
    public AudioSource[] jsSound;
    // เรียกฟังก์ชันนี้เพื่อทำ JumpScare
    public void JumpScare()
    {
        int randomIndex = Random.Range(0, jsSound.Length);

        AudioSource chosenSound = jsSound[randomIndex];

        chosenSound.PlayOneShot(chosenSound.clip);

        StartCoroutine(JumpScareCoroutine());
    }

    private IEnumerator JumpScareCoroutine()
    {
        // 1️⃣ เริ่มสั่นกล้อง
        StartCoroutine(ShakeCamera(shakeDuration));

        // 2️⃣ เปลี่ยนสี Sprite ของทุก object ที่มี tag "room"
       
        for(int i = 0; i < room.Length;i++)
        {
            room[i].color = new Color(1, 0, 0, 1);
        }

        // 3️⃣ รอ 2 วินาที
        yield return new WaitForSeconds(2f);

        // 4️⃣ กลับสี Sprite เป็น ขาว
        for (int i = 0; i < room.Length; i++)
        {
            room[i].color = new Color(1, 1, 1, 1);
        }

        // 5️⃣ กล้องหยุดสั่น (ShakeCamera Coroutine จะหยุดเองเมื่อครบเวลา)
    }

    private IEnumerator ShakeCamera(float duration)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;
        isJumpscare = true;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
           // allUI.SetActive(false);

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        //    allUI.SetActive(true);
        isJumpscare = false;

        mainCamera.transform.localPosition = originalPos; // คืนตำแหน่งเดิม
    }
}
