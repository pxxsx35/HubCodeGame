using UnityEngine;

public class PositionHelper : MonoBehaviour
{
    [ContextMenu("เพิ่มตำแหน่ง 21 ตำแหน่ง")]
    public void AddAllPositions()
    {
        PositionItem posItem = GetComponent<PositionItem>();
        
        if (posItem == null)
        {
            Debug.LogError("ไม่มี PositionItem Component!");
            return;
        }
        
        // ล้างตำแหน่งเก่า
        posItem.positions.Clear();
        
        // เพิ่มตำแหน่งทั้งหมด
        posItem.positions.Add(new ItemPosition { position = new Vector4(5f, 4.2f, 1, 2) }); // 1
        posItem.positions.Add(new ItemPosition { position = new Vector4(0.45f, 3.4f, 1, 2) }); // 2
        //posItem.positions.Add(new ItemPosition { position = new Vector4(-4.5f, 2.32f, 2, 2) }); // 3
        posItem.positions.Add(new ItemPosition { position = new Vector4(-4.6f, 2.32f, 2, 2) }); // 4
        posItem.positions.Add(new ItemPosition { position = new Vector4(-2.1f, 1.5f, 1, 2) }); // 5
        posItem.positions.Add(new ItemPosition { position = new Vector4(2.15f, 0.2f, 1, 2) }); // 6
        posItem.positions.Add(new ItemPosition { position = new Vector4(1.68f, -1.95f, 1, 2) }); // 7
        posItem.positions.Add(new ItemPosition { position = new Vector4(-4.32f, -4.15f, 1, 2) }); // 8
        posItem.positions.Add(new ItemPosition { position = new Vector4(8.08f, 1.03f, 2, 1) }); // 9
        posItem.positions.Add(new ItemPosition { position = new Vector4(4.43f, -2f, 2, 1) }); // 10
        posItem.positions.Add(new ItemPosition { position = new Vector4(2.62f, 1.78f, 1, 1) }); // 11
        posItem.positions.Add(new ItemPosition { position = new Vector4(0.6f, -3.46f, 1, 1) }); // 12
        posItem.positions.Add(new ItemPosition { position = new Vector4(-3.94f, -1.21f, 2, 1) }); // 13
        posItem.positions.Add(new ItemPosition { position = new Vector4(-7.37f, 1.29f, 2, 1) }); // 14
        posItem.positions.Add(new ItemPosition { position = new Vector4(0, 4.5f, 1, 3) }); // 15
        posItem.positions.Add(new ItemPosition { position = new Vector4(-5.4f, 2.3f, 2, 3) }); // 16
        posItem.positions.Add(new ItemPosition { position = new Vector4(-5.3f, -1f, 2, 3) }); // 17
        posItem.positions.Add(new ItemPosition { position = new Vector4(6.66f, 0.8f, 2, 3) }); // 18
        posItem.positions.Add(new ItemPosition { position = new Vector4(3.7f, 4.4f, 1, 3) }); // 19
        posItem.positions.Add(new ItemPosition { position = new Vector4(2.8f, -1f, 1, 3) }); // 20
        posItem.positions.Add(new ItemPosition { position = new Vector4(6.44f, -1.44f, 2, 3) }); // 21
        
        Debug.Log($"✅ เพิ่มตำแหน่งสำเร็จ! รวม {posItem.positions.Count} ตำแหน่ง");
        
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(posItem);
        #endif
    }
}