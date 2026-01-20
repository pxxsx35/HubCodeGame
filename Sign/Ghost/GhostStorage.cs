using UnityEngine;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{
    public Vector2[] position;
    public GhostDatabase database;

    private Dictionary<string, GameObject> spawnedGhostObjects = new Dictionary<string, GameObject>();
    private int posIndex = 0;

    private void Update()
    {
        SpawnChosenGhosts();
    }

    private void SpawnChosenGhosts()
    {
        if (database == null || database.allGhosts == null)
            return;

        foreach (GhostData ghost in database.allGhosts)
        {
            if (ghost.isShow)
            {
                if (!spawnedGhostObjects.ContainsKey(ghost.name))
                {
                    if (posIndex >= position.Length)
                    {
                        Debug.LogWarning("ตำแหน่ง spawn หมดแล้ว!");
                        return;
                    }

                    Vector2 spawnPos = position[posIndex];
                    GameObject newGhost = Instantiate(ghost.ghostPrefab, spawnPos, Quaternion.identity);

                    // ✅ เปลี่ยน Tag เป็น Default
                    newGhost.tag = "Untagged";

                    // ✅ ลบทุก Component ที่ไม่ใช่ SpriteRenderer
                    RemoveAllComponentsExceptSprite(newGhost);

                    // ✅ เพิ่ม Script ShowGhost เข้าไป
                    newGhost.AddComponent<ShowGhostChoosed>();

                    // ✅ ปิดลูกตัวแรก (ถ้ามี)
                    if (newGhost.transform.childCount > 0)
                        newGhost.transform.GetChild(0).gameObject.SetActive(false);

                    spawnedGhostObjects.Add(ghost.name, newGhost);
                    posIndex++;
                }
            }
        }
    }

    private void RemoveAllComponentsExceptSprite(GameObject obj)
    {
        Component[] comps = obj.GetComponents<Component>();
        foreach (Component c in comps)
        {
            if (!(c is Transform) && !(c is SpriteRenderer))
            {
                Destroy(c);
            }
        }
    }
}
