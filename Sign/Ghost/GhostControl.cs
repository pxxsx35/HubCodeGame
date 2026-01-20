using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class GhostControl : MonoBehaviour
{
    public GhostDatabase database;
    public TypewriterEffectUI typewriter;
    public Vector2 positionQone;
    public Vector2 positionQtwo;
    public Vector2 positiondialoge;
    public int fontSize;
    private ShowGhost showGhost;
    private PlayerStatus player;
    private MouseTexture mouse;
    private int indexGhost;
    private bool isAnwser;
    [SerializeField] Sprite[] room;
    private GameObject allUI;
    // Start is called before the first frame update
    void Start()
    {
        
        mouse = FindObjectOfType<MouseTexture>();
        showGhost = FindAnyObjectByType<ShowGhost>();
        player = FindAnyObjectByType<PlayerStatus>();
        for (int i = 0; i < database.allGhosts.Count; i++)
        {
            if (database.allGhosts[i].ghostName == gameObject.name)
            {
                indexGhost = i;
            }
            database.allGhosts[i].isChoose = false;

         
        }

        //ChangeData
        gameObject.transform.GetChild(0).GetChild(3).transform.localPosition = positiondialoge;
        gameObject.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Text>().fontSize = fontSize;
        gameObject.transform.GetChild(0).GetChild(1).transform.localPosition = positionQtwo;
        gameObject.transform.GetChild(0).GetChild(0).transform.localPosition = positionQone;


        typewriter = GameObject.Find("textGhostUI").GetComponent<TypewriterEffectUI>();
        if (typewriter != null)
            typewriter.textComponent = gameObject.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Text>();
      




    }

    // Update is called once per frame
    void Update()
    {
       
            if (typewriter.textComponent.text != "")
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            else
                gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);

        GameObject check = GameObject.Find("DialogeStoryBG");
        TypewriterEffectUI type = GameObject.Find("DialogeStory").GetComponent<TypewriterEffectUI>();
        if ((type != null && type.IsTyping()) || (check != null && check.activeSelf == true))
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
           
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

        }

        if(isAnwser || player.fear >= player.maxFear)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        GameObject check = GameObject.Find("DialogeStoryBG");
        TypewriterEffectUI type = GameObject.Find("DialogeStory").GetComponent<TypewriterEffectUI>();
        if ((type != null && type.IsTyping()) || (check != null && check.activeSelf == true)) return;

            // ถ้าข้อความกำลังพิมพ์ → Skip ก่อน
            if (typewriter.IsTyping())
        {
            typewriter.SkipText();
            return;
        }

        if (!database.allGhosts[indexGhost].isChoose)
        {
            if (!showGhost.isGhost && database.allGhosts[indexGhost].ghostStage == 0)
            {
                // วันสุดท้ายของ Ghost
                if (database.allGhosts[indexGhost].ghostDay == 7)
                {
                    HandleEndOfDayGhost();
                }
                else
                {
                    HandleNormalGhostClick();
                }
            }

            HandleGhostDialogueProgress();
        }

        Debug.Log(gameObject.name);
    }

    private void answerOne()
    {
        HandleAnswerSelected(0);
    }

    private void answerTwo()
    {
        HandleAnswerSelected(1);
    }

    private void HandleAnswerSelected(int answerIndex)
    {
        // Skip ถ้ากำลังพิมพ์
        if (typewriter.IsTyping())
        {
            typewriter.SkipText();
            return;
        }

        database.allGhosts[indexGhost].ghostStage += 1;

        // ปิดปุ่มคำตอบ
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        isAnwser = true;
        // แสดงข้อความแบบ typewriter
        typewriter.ShowText(database.allGhosts[indexGhost].answer[answerIndex]);
    }

    private void HandleGhostDialogueProgress()
    {
        int stage = database.allGhosts[indexGhost].ghostStage;

        if (stage < database.allGhosts[indexGhost].answer.Length - 1)
        {
            if (stage >= 1)
            {
                database.allGhosts[indexGhost].ghostStage += 1;
                typewriter.ShowText(database.allGhosts[indexGhost].answer[database.allGhosts[indexGhost].ghostStage]);
            }
        }
        else
        {
            // ปิด Ghost หลังข้อความครบ
            typewriter.StopTypingImmediate();
            Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);

            database.allGhosts[indexGhost].ghostStage = 0;
            showGhost.isGhost = false;
            transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            isAnwser = true;

            gameObject.SetActive(false);
        }
    }

    private void HandleEndOfDayGhost()
    {
        if (database.allGhosts[indexGhost].ghostType != "good")
        {
            player.WakeUp();
            DeleteAllGhosts();
            Debug.Log("💀 ลบผีทั้งหมดเรียบร้อยแล้ว");
        }
        else
        {
            DeleteAllGhosts();
            FindObjectOfType<RoomFadeManager>().StartFadeOutAllRooms();
        }
    }

    private void HandleNormalGhostClick()
    {
        database.allGhosts[indexGhost].isChoose = true;
        database.allGhosts[indexGhost].isShow = true;
        showGhost.isChoose = true;

        if (database.allGhosts[indexGhost].ghostType != "good")
        {
            player.fear += player.damage;
            FindObjectOfType<JumpScareManager>().JumpScare();
        }
        else
        {
            player.fear -= player.damage;
        }

        // ลบผีทั้งหมดบนฉาก
        DeleteAllGhosts();

        // สุ่มวันให้ Ghost ที่ยังไม่เลือก
        for (int i = 0; i < database.allGhosts.Count; i++)
        {
            if (!database.allGhosts[i].isChoose && database.allGhosts[i].ghostDay == showGhost.time.dayCount && database.allGhosts[i].ghostType == "bad")
            {
                database.allGhosts[i].ghostDay = Random.Range(4, 7);
            }
        }
    }

    private void DeleteAllGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject g in ghosts)
        {
            Destroy(g);
        }
    }


}
