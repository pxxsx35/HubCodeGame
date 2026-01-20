using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class closetext : MonoBehaviour
{
    public float flashInterval = 0.5f; // ความเร็วในการกระพริบ (วินาที)
    private Text textComponent;

    void Start()
    {
        textComponent = GetComponent<Text>(); // อ้างอิง Text component ของ GameObject นี้
        StartCoroutine(Flash()); // เริ่ม Coroutine ที่จะทำให้ Text กระพริบ
    }

    // Coroutine ที่จะทำให้ Text กระพริบ
    IEnumerator Flash()
    {
        while (true) // ทำแบบไม่รู้จบ (กระพริบตลอด)
        {
            textComponent.enabled = !textComponent.enabled; // สลับสถานะการแสดงผล
            yield return new WaitForSeconds(flashInterval); // รอให้เวลาในแต่ละครั้งก่อนกระพริบครั้งต่อไป
        }
    }
}
