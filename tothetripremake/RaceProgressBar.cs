using UnityEngine;
using UnityEngine.UI;

public class RaceProgressBar : MonoBehaviour
{
    [Header("References")]
    public Transform playerVehicle;    // ลากตัวรถมาใส่
    public Transform finishLine;      // ลากจุดเส้นชัยมาใส่
    public Slider progressSlider;     // ลาก UI Slider มาใส่

    private float startX;
    private float totalDistance;

    void Start()
    {
        // บันทึกจุดเริ่มต้น และระยะทางรวมในแกน X
        startX = playerVehicle.position.x;
        totalDistance = finishLine.position.x - startX;
    }

    void Update()
    {
        // คำนวณระยะทางที่วิ่งมาได้ปัจจุบัน
        float currentDistance = playerVehicle.position.x - startX;

        // คำนวณเป็นค่า 0 ถึง 1 (ใช้ Mathf.Clamp เพื่อไม่ให้ค่าเกินถ้าวิ่งเลยเส้นชัย)
        float progress = Mathf.Clamp01(currentDistance / totalDistance);

        // อัปเดต UI Slider
        if (progressSlider != null)
        {
            progressSlider.value = progress;
        }
    }
}