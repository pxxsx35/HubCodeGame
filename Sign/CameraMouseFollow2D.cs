using UnityEngine;

public class CameraMouseFollow2D : MonoBehaviour
{
    [Header("Scene Limit (minX, maxX)")]
    public Vector2 limitX;

    [Header("Scene Limit (minY, maxY)")]
    public Vector2 limitY;

    [Header("Mouse Settings")]
    public float mouseFollowStrength = 5f; // ความแรงของการขยับตามเมาส์
    public float smoothSpeed = 5f;          // ความนุ่มนวลของกล้อง

    private Vector3 targetPos;

    void Start()
    {
        targetPos = transform.position;
    }

    private void Update()
    {
        // อ่านการเคลื่อนของเมาส์
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // คำนวณตำแหน่งเป้าหมายของกล้อง
        targetPos.x += mouseX * mouseFollowStrength * Time.deltaTime;
        targetPos.y += mouseY * mouseFollowStrength * Time.deltaTime;

        // จำกัดตำแหน่งกล้องไม่ให้ออกนอกขอบ
        targetPos.x = Mathf.Clamp(targetPos.x, limitX.x, limitX.y);
        targetPos.y = Mathf.Clamp(targetPos.y, limitY.x, limitY.y);

        // เคลื่อนกล้องไปยังตำแหน่งเป้าหมายแบบนุ่มนวล
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }

    // แสดงกรอบใน Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(
            (limitX.x + limitX.y) / 2f,
            (limitY.x + limitY.y) / 2f,
            transform.position.z
        );
        Vector3 size = new Vector3(
            limitX.y - limitX.x,
            limitY.y - limitY.x,
            0f
        );
        Gizmos.DrawWireCube(center, size);
    }
}
