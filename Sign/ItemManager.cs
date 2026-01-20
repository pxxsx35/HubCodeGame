using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("ไอเทมทั้งหมด")]
    [Tooltip("ไอเทมแต่ละชิ้นมี assignedDay กำหนดมาแล้ว")]
    public GameObject[] itemPrefabs;

    [Header("ตำแหน่งที่วางได้")]
    public PositionItem[] positionContainers;
    public Transform[] parentTransform;

    [Header("Room & Time System")]
    public ChangeRoom roomController;
    public TimeCount timeCounter;

    [Header("ตั้งค่าระบบวัน")]
    [Tooltip("จำนวนวันทั้งหมด")]
    public int totalDays = 7;

    [Header("Debug")]
    public bool autoSpawnOnStart = true;
    public bool respawnOnNewDay = true;  // ⭐ สุ่มตำแหน่งใหม่ทุกวัน

    private List<GameObject> currentDayItems = new List<GameObject>();  // ⭐ ไอเทมของวันปัจจุบัน
    private List<ItemPosition> allPositions = new List<ItemPosition>();
    private int lastSpawnedDay = -1;

    // ⭐ เก็บไอเทม Prefab แยกตามวัน (ไม่ใช่ GameObject ที่สร้างแล้ว)
    private Dictionary<int, List<GameObject>> itemPrefabsByDay = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        if (roomController == null)
            roomController = FindObjectOfType<ChangeRoom>();

        if (timeCounter == null)
            timeCounter = FindObjectOfType<TimeCount>();

        CollectAllPositions();

        if (autoSpawnOnStart)
        {
            GroupItemsByAssignedDay();  // ⭐ จัดกลุ่มไอเทมตาม assignedDay
            SpawnItemsForCurrentDay();  // ⭐ สร้างไอเทมวันที่ 1
        }
    }

    void Update()
    {
        // ⭐ ตรวจสอบว่าเปลี่ยนวันหรือไม่
        if (respawnOnNewDay && timeCounter != null && timeCounter.dayCount != lastSpawnedDay)
        {
            if (timeCounter.dayCount <= totalDays)
            {
                SpawnItemsForCurrentDay();  // ⭐ สุ่มตำแหน่งใหม่
            }
        }
    }

    void CollectAllPositions()
    {
        allPositions.Clear();

        foreach (var container in positionContainers)
        {
            if (container != null)
                allPositions.AddRange(container.positions);
        }
    }

    // ⭐⭐ จัดกลุ่มไอเทมตาม assignedDay ที่กำหนดมาแล้ว
    void GroupItemsByAssignedDay()
    {
        if (itemPrefabs.Length == 0)
            return;

        itemPrefabsByDay.Clear();

        foreach (GameObject prefab in itemPrefabs)
        {
            if (prefab == null) continue;

            Item itemScript = prefab.GetComponent<Item>();
            if (itemScript == null) continue;

            int assignedDay = itemScript.GetDay();
            if (assignedDay < 1 || assignedDay > totalDays) continue;

            if (!itemPrefabsByDay.ContainsKey(assignedDay))
                itemPrefabsByDay[assignedDay] = new List<GameObject>();

            itemPrefabsByDay[assignedDay].Add(prefab);
        }
    }

    // ⭐⭐ สร้างไอเทมสำหรับวันปัจจุบัน (สุ่มตำแหน่งใหม่ทุกครั้ง!)
    void SpawnItemsForCurrentDay()
    {
        ClearCurrentDayItems();  // ลบของเก่า

        if (timeCounter == null) return;
        int currentDay = timeCounter.dayCount;

        if (currentDay < 1 || currentDay > totalDays) return;
        if (!itemPrefabsByDay.ContainsKey(currentDay) || itemPrefabsByDay[currentDay].Count == 0)
        {
            lastSpawnedDay = currentDay;
            return;
        }
        if (allPositions.Count < 1) return;

        foreach (var container in positionContainers)
            if (container != null) container.ResetAllOccupied();

        List<GameObject> todayPrefabs = itemPrefabsByDay[currentDay];

        for (int i = 0; i < todayPrefabs.Count; i++)
        {
            GameObject itemPrefab = todayPrefabs[i];
            Item itemScript = itemPrefab.GetComponent<Item>();
            if (itemScript == null) continue;

            string itemName = itemScript.itemName;

            // ป้องกันซ้ำชื่อ
            bool alreadyExists = currentDayItems.Any(x =>
            {
                Item existing = x.GetComponent<Item>();
                return existing != null && existing.itemName == itemName;
            });
            if (alreadyExists) continue;

            // หาตำแหน่งว่างที่เหมาะสม
            ItemPosition validPosition = GetRandomValidPosition(itemScript.displayType);
            if (validPosition == null) continue;

            // หาพาเรนต์จากห้อง
            int index = validPosition.Room switch
            {
                1 => 0,
                2 => 1,
                _ => 2
            };

            GameObject newItem = Instantiate(itemPrefab, parentTransform[index]);
            newItem.transform.position = new Vector3(validPosition.X, validPosition.Y, 0f);
            newItem.name = $"{itemPrefab.name}_Day{currentDay}_Room{validPosition.Room}";

            // ตั้งค่าไอเทม
            Item newItemScript = newItem.GetComponent<Item>();
            newItemScript.SetDisplayType(validPosition.DisplayType);
            newItemScript.SetRoom(validPosition.Room);
            newItemScript.SetDay(currentDay);
            newItemScript.roomController = roomController;
            newItemScript.timeCounter = timeCounter;

            // ตั้งค่า Sorting
            SpriteRenderer sr = newItem.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Default";
                sr.sortingOrder = 2;
            }

            // ⭐⭐ ส่วนที่เพิ่มมา — บังคับ FadeOnEnable ทำงานหลังสร้างเสร็จ
            FadeOnEnable[] fades = newItem.GetComponentsInChildren<FadeOnEnable>(true);
            foreach (var fade in fades)
            {
                fade.enabled = false;  // ปิดก่อน
                fade.enabled = true;   // เปิดใหม่ให้ OnEnable() รันอีกครั้ง
            }

            // เก็บข้อมูล
            validPosition.isOccupied = true;
            currentDayItems.Add(newItem);
        }

        lastSpawnedDay = currentDay;
    }


    ItemPosition GetRandomValidPosition(int displayType)
    {
        var validPositions = allPositions
            .Where(pos => pos != null && pos.DisplayType == displayType && !pos.isOccupied)
            .ToList();

        if (validPositions.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validPositions.Count);
        return validPositions[randomIndex];
    }

    // ⭐ ลบเฉพาะไอเทมของวันปัจจุบัน
    public void ClearCurrentDayItems()
    {
        foreach (var item in currentDayItems)
        {
            if (item != null)
                Destroy(item);
        }
        currentDayItems.Clear();
    }

    [ContextMenu("สุ่มไอเทมใหม่ทั้งหมด")]
    void RandomizeItems()
    {
        GroupItemsByAssignedDay();
        SpawnItemsForCurrentDay();
    }

    [ContextMenu("สุ่มตำแหน่งวันนี้ใหม่")]
    void RespawnCurrentDay()
    {
        SpawnItemsForCurrentDay();
    }
}
