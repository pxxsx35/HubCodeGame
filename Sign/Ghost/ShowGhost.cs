using System.Collections.Generic;
using UnityEngine;

public class ShowGhost : MonoBehaviour
{
    public GhostDatabase database;
    public TimeCount time;
    public PlayerStatus player;
    [Range(1, 5)] public int ghostToShow = 1;

    private List<GameObject> spawnedGhosts = new List<GameObject>();
    private List<int> alreadyShownIndexes = new List<int>(); // เก็บ index ผีที่เคยแสดง
    private bool isSpawned = false;
    public bool isGhost = false;
    public bool isChoose = false;
    public SpriteRenderer outSide;
    private int ghostCount;
    private int currentRandomIndex = -1;
    private int lastDay = -1; // เพิ่มไว้ด้านบนสุดของ class


    private void Start()
    {
        for (int i = 0; i < database.allGhosts.Count; i++)
        {
            database.allGhosts[i].ghostDay = database.allGhosts[i].ghostRealDay;
            database.allGhosts[i].isShow = false;
        }
    }

    void Update()
    {
       
        if (time.dayCount != lastDay)
        {
            ResetForNewDay();
            lastDay = time.dayCount;
        }
        // 1️⃣ Instantiate ผีทั้งหมดครั้งแรก
        if (!isSpawned)
        {
            for (int i = 0; i < database.allGhosts.Count; i++)
            {
                GhostData ghost = database.allGhosts[i];
                if (time.dayCount == ghost.ghostDay && ghost.ghostPrefab != null)
                {
                    if (ghostCount <= 4)
                    {
                        ghostCount++;

                        // 1️⃣ Instantiate Ghost
                        GameObject ghostObj = Instantiate(
                            ghost.ghostPrefab,
                            new Vector2(1.5f, -1f),
                            Quaternion.identity
                        );

                        ghostObj.name = ghost.ghostName;
                        ghostObj.SetActive(false);
                        spawnedGhosts.Add(ghostObj);

                        // 2️⃣ สร้าง outsideObj เป็น child ของ Ghost
                        if (outSide != null)
                        {
                            GameObject outsideObj = new GameObject(ghostObj.name + "_Outside");
                            SpriteRenderer sr = outsideObj.AddComponent<SpriteRenderer>();
                            sr.sprite = outSide.sprite;

                            // ตั้ง sortingOrder
                            sr.sortingOrder = 5;

                            // ตั้งตำแหน่งให้ตรงผี
                            outsideObj.transform.position = ghostObj.transform.position;

                            // ตั้งเป็น child ของ Ghost
                            outsideObj.transform.SetParent(ghostObj.transform);

                            // ปรับ scale ให้ใหญ่พอดีกับ Ghost
                            SpriteRenderer ghostSR = ghostObj.GetComponent<SpriteRenderer>();
                            if (ghostSR != null && ghostSR.sprite != null)
                            {
                                Vector2 ghostSize = ghostSR.sprite.bounds.size;
                                Vector2 outsideSize = sr.sprite.bounds.size;
                                Vector3 newScale = outsideObj.transform.localScale;
                                newScale.x = ghostSize.x / outsideSize.x * 3.5f;
                                newScale.y = ghostSize.y / outsideSize.y * 1.5f;
                                outsideObj.transform.localScale = newScale;
                            }

                            // ปิดการแสดงก่อน
                            outsideObj.SetActive(false);
                        }
                    }
                }
            }


            isSpawned = true;
        }

        // 2️⃣ ถ้าเข้าสู่ฝันและไม่มีผีปรากฏ → สุ่มผีใหม่ที่ยังไม่เคยแสดง
        if (player.isDream && !isGhost && spawnedGhosts.Count > 0)
        {
            // สร้าง list ของ index ที่ยังไม่เคยแสดง
            List<int> availableIndexes = new List<int>();
            for (int i = 0; i < spawnedGhosts.Count; i++)
            {
                if (!alreadyShownIndexes.Contains(i))
                    availableIndexes.Add(i);
            }

            if (availableIndexes.Count > 0)
            {
                currentRandomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];

                for (int i = 0; i < spawnedGhosts.Count; i++)
                {
                    if (i == currentRandomIndex)
                    {
                        spawnedGhosts[i].SetActive(true);
                        isGhost = true;
                        alreadyShownIndexes.Add(i); // บันทึกว่าผีตัวนี้แสดงแล้ว
                     //   Debug.Log($"👻 Ghost appeared: {spawnedGhosts[i].name}");
                    }
                    else
                    {
                        spawnedGhosts[i].SetActive(false);
                    }
                }
            }
            else
            {
                if(availableIndexes != null)
                {
                    if(!isGhost && !isChoose)
                    {
                        
                        for(int i = 0;i < ghostCount;i++)
                        {
                            

                                spawnedGhosts[i].SetActive(true);
                                float totalWidth = 10f;              // ความกว้างทั้งหมด
                                                                     // จำนวนผีทั้งหมด
                                float ghostWidth = totalWidth / ghostCount;  // ความกว้างของผีแต่ละตัว

                                // คำนวณตำแหน่งให้อยู่ตรงกลางพอดี (เริ่มจากซ้ายสุด -7 ถึงขวาสุด +7)
                                float startX = -totalWidth / 2f + ghostWidth / 2f;
                                float posX = startX + i * ghostWidth;
                                
                                spawnedGhosts[i].transform.position = new Vector2(posX, 0);
                                spawnedGhosts[i].transform.localScale = (Vector3.one) / (ghostCount * 2);
                            spawnedGhosts[i].transform.GetChild(1).gameObject.SetActive(true);
                            //availableIndexes.Remove(i);

                       


                        }
                        if(isChoose)
                        {
                          //Destroy()
                        }

                    }
                    else
                    {
                      
                        ghostCount = 0;
                        player.WakeUp();
                        isChoose = false;
                        isSpawned = false;
                       
                    }

   


                }




            }
        }

        // 3️⃣ เมื่อออกจากฝัน → ปิดผีทั้งหมด
        if (!player.isDream && isGhost)
        {
            foreach (GameObject ghost in spawnedGhosts)
            {
                ghost.SetActive(false);
            }
            isGhost = false;
            currentRandomIndex = -1;
        }

      

    }

    public void ResetGhosts()
    {
        alreadyShownIndexes.Clear();
       
    }
    private void ResetForNewDay()
    {
        // ลบผีเก่าทั้งหมด
        foreach (GameObject ghost in spawnedGhosts)
        {
            if (ghost != null)
            {
                Debug.Log(ghost.name);

                Destroy(ghost);

            }
        }

     

        spawnedGhosts.Clear();
        alreadyShownIndexes.Clear();
        isSpawned = false;
        isGhost = false;
        isChoose = false;
        ghostCount = 0;
        currentRandomIndex = -1;

   
    }

}
