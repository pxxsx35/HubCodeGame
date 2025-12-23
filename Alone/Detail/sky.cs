using UnityEngine;

public class sky : MonoBehaviour
{
    public Transform player;  // อ้างอิงตำแหน่งผู้เล่น
    public float smoothSpeed = 0.1f; // ความเร็วในการติดตาม
    public Vector3 offset;  // ใช้สำหรับปรับตำแหน่งท้องฟ้าให้ห่างจากผู้เล่น
    private Vector3 velocity = Vector3.zero;  // ใช้สำหรับ smooth movement

    void Update()
    {
        // กำหนดตำแหน่งเป้าหมายให้ห่างจากตำแหน่งผู้เล่นเล็กน้อย
        Vector3 targetPosition = player.position + offset;

        // ใช้ SmoothDamp เพื่อให้ท้องฟ้าเคลื่อนที่ไปตามผู้เล่นอย่างนุ่มนวล
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
    }
}
