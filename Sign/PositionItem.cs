using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemPosition
{
    [Header("ตำแหน่งที่วางได้")]
    public Vector4 position;  // x, y = พิกัด, z = ประเภท (1,2), w = ห้อง (1,2,3)
    
    [HideInInspector]
    public bool isOccupied = false;  // ตำแหน่งนี้มีไอเทมแล้วหรือยัง
    
    // ดึงข้อมูลแยกออกมา
    public float X => position.x;
    public float Y => position.y;
    public int DisplayType => (int)position.z;  // 1=หันตรง, 2=หันข้าง
    public int Room => (int)position.w;         // 1, 2, 3
    
    public string GetInfo()
    {
        string type = DisplayType == 1 ? "หันตรง" : "หันข้าง";
        return $"({X}, {Y}) ห้อง {Room} {type}";
    }
}
//---------------------------------------------------------------------
public class PositionItem : MonoBehaviour
{
    private PositionHelper positionHelp;

    [Header("ตำแหน่งทั้งหมด")]
    [Tooltip("เพิ่มตำแหน่งได้หลายตำแหน่งในนี้")]
    public List<ItemPosition> positions = new List<ItemPosition>();
    
    [Header("ตั้งค่าการแสดงผล")]
    public bool showGizmos = true;
    public float gizmoSize = 0.3f;
    public Color availableColor = Color.green;
    public Color occupiedColor = Color.red;



    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        
        // แสดงตำแหน่งทั้งหมดใน Scene Editor
        foreach (var pos in positions)
        {
            if (pos == null) continue;
            
            Gizmos.color = pos.isOccupied ? occupiedColor : availableColor;
            Vector3 worldPos = new Vector3(pos.X, pos.Y, 0);
            
            // วงกลม
            Gizmos.DrawWireSphere(worldPos, gizmoSize);
            
            // เส้นแสดงทิศทาง (หันตรง = ขึ้น, หันข้าง = ขวา)
            if (pos.DisplayType == 1)
            {
                Gizmos.DrawLine(worldPos, worldPos + Vector3.up * gizmoSize);
            }
            else
            {
                Gizmos.DrawLine(worldPos, worldPos + Vector3.right * gizmoSize);
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        
        // แสดงข้อมูลเมื่อเลือก GameObject
        foreach (var pos in positions)
        {
            if (pos == null) continue;
            
            Vector3 worldPos = new Vector3(pos.X, pos.Y, 0);
            
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(worldPos + Vector3.up * 0.5f, pos.GetInfo());
            #endif
        }
    }
    
    // ฟังก์ชันช่วยเพิ่มตำแหน่งจาก code
    public void AddPosition(float x, float y, int displayType, int room)
    {
        positions.Add(new ItemPosition
        {
            position = new Vector4(x, y, displayType, room)
        });
    }
    
    // รีเซ็ตสถานะทั้งหมด
    public void ResetAllOccupied()
    {
        foreach (var pos in positions)
        {
            if (pos != null)
                pos.isOccupied = false;
        }
    }
    
    // นับตำแหน่งว่าง
    public int GetAvailableCount()
    {
        int count = 0;
        foreach (var pos in positions)
        {
            if (pos != null && !pos.isOccupied)
                count++;
        }
        return count;
    }
    
    // แสดงสถิติ
    [ContextMenu("แสดงสถิติตำแหน่ง")]
    void ShowStats()
    {
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log($"📊 สถิติตำแหน่งใน {gameObject.name}");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log($"รวมทั้งหมด: {positions.Count} ตำแหน่ง");
        Debug.Log($"ว่าง: {GetAvailableCount()} ตำแหน่ง");
        Debug.Log($"ถูกใช้แล้ว: {positions.Count - GetAvailableCount()} ตำแหน่ง");
        
        var byRoom = new Dictionary<int, int>();
        var byType = new Dictionary<int, int>();
        
        foreach (var pos in positions)
        {
            if (pos == null) continue;
            
            if (!byRoom.ContainsKey(pos.Room))
                byRoom[pos.Room] = 0;
            byRoom[pos.Room]++;
            
            if (!byType.ContainsKey(pos.DisplayType))
                byType[pos.DisplayType] = 0;
            byType[pos.DisplayType]++;
        }
        
        Debug.Log("\nแบ่งตามห้อง:");
        foreach (var kvp in byRoom)
        {
            Debug.Log($"  ห้อง {kvp.Key}: {kvp.Value} ตำแหน่ง");
        }
        
        Debug.Log("\nแบ่งตามประเภท:");
        foreach (var kvp in byType)
        {
            string typeName = kvp.Key == 1 ? "หันตรง" : "หันข้าง";
            Debug.Log($"  {typeName}: {kvp.Value} ตำแหน่ง");
        }
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━");
    }
}