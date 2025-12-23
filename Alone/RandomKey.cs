using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomKey : MonoBehaviour
{
    [SerializeField] private Sprite[] keys;
    [SerializeField] private List<Vector2> positions;
    public int[] password = new int[4]; //Password
    private Quest quest;

    public List<int> usedPositionIndices = new List<int>(); // เก็บตำแหน่งที่สุ่มไปแล้ว
    private int lastQuestIndex = -1;
    private Vector2 lastPosition;

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();

        // สุ่มค่าเริ่มต้นสำหรับ p[]
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = Random.Range(1, 10);
        }
    }

    public int randomKey(int i)
    {
        int indexPos = -1;

        if (i == lastQuestIndex && quest.questCompleted[3] == false)
        {
            
            transform.GetChild(i).position = lastPosition;
            return usedPositionIndices[i]; // คืนค่า index ที่ใช้ก่อนหน้า
        }
        else
        {
            if (positions.Count > usedPositionIndices.Count)
            {
                do
                {
                    indexPos = Random.Range(0, positions.Count);
                } while (usedPositionIndices.Contains(indexPos)); // หาตำแหน่งที่ยังไม่ถูกใช้

                lastPosition = positions[indexPos];
                transform.GetChild(i).position = lastPosition;
                usedPositionIndices.Add(indexPos); // เก็บ index ที่ถูกใช้แล้ว
            }
            else
            {
                Debug.LogWarning("ตำแหน่งหมดแล้ว! ไม่สามารถสุ่มได้อีก");
            }

            lastQuestIndex = i;
        }

        if (quest.questCompleted[3] == false)
        {
            Transform child = transform.GetChild(i);

            child.GetComponent<SpriteRenderer>().sprite = keys[password[i]];
            child.GetComponent<BoxCollider2D>().size = child.GetComponent<SpriteRenderer>().size;
        }

        return indexPos; // คืนค่า index ที่สุ่มมา
    }
}
