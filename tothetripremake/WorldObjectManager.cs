using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    [Header("Settings")]
    public Transform playerTransform; // ลากตัวรถมาใส่
    public float activeDistance = 100f; // ระยะที่เข้าใกล้แล้วให้ "เปิด"
    public float passedDistance = 20f;  // ระยะที่ขับผ่านไปแล้ว (ข้างหลัง) ให้ "ลบ"

    [Header("Object List")]
    public List<GameObject> mapObjects = new List<GameObject>(); // เก็บ Object ทั้งหมดในด่าน


    private void Start()
    {
        StartCoroutine(ManageObjectsRoutine());
    }

    IEnumerator ManageObjectsRoutine()
    {
        while (true)
        {
            
            ManageWorldObjects();
            yield return new WaitForSeconds(0.2f); 
        }
    }

    private void ManageWorldObjects()
    {
        for (int i = mapObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = mapObjects[i];

            // ตรวจสอบว่า Object ยังมีตัวตนอยู่หรือไม่
            if (obj == null)
            {
                mapObjects.RemoveAt(i);
                continue;
            }

            // คำนวณระยะห่างบนแกน X (อ้างอิงจาก Logic เกมขับรถของคุณ)
            float xDistance = obj.transform.position.x - playerTransform.position.x;

            // 1. ถ้าขับผ่านไปแล้ว (อยู่ข้างหลังผู้เล่น เกินระยะที่กำหนด)
            if (xDistance < -passedDistance)
            {
                mapObjects.RemoveAt(i);
                Destroy(obj); // ลบทิ้งเพื่อคืน Memory
                continue;
            }

            // 2. ถ้าอยู่ข้างหน้า แต่ระยะห่างเกินกว่าที่จะมองเห็น
            if (Mathf.Abs(xDistance) > activeDistance)
            {
                if (obj.activeSelf) obj.SetActive(false); // ปิดการใช้งานเพื่อลด Draw Calls
            }
            // 3. ถ้าอยู่ในระยะที่ควรมองเห็น
            else
            {
                if (!obj.activeSelf) obj.SetActive(true); // เปิดการใช้งาน
            }
        }

    }


}