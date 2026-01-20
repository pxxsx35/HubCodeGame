using UnityEngine;

public class ShowGhostChoosed : MonoBehaviour
{
    ChangeRoom room;
    private SpriteRenderer sprite;

    void Start()
    {
        room = FindObjectOfType<ChangeRoom>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (room.roomInt == 4)
        {
            sprite.enabled = true;  // ✅ แค่เปิดภาพ
        }
        else
        {
            sprite.enabled = false; // ❌ ซ่อนภาพ แต่ Script ยังทำงานอยู่
        }

        if (transform.position.y < -1)
        {
            sprite.sortingOrder = 5; // ถ้าอยู่ต่ำกว่า 0 → ลำดับสูงกว่า
        }
        else
        {
            sprite.sortingOrder = 4; // ค่าเริ่มต้น
        }
    }
}
