using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [SerializeField] string[] letter;
    TypewriterEffectUI type;
    MouseTexture mouse;
    ChangeRoom room;

    private PlayerStatus player;
    private SpriteRenderer sr;
    private List<int> shownIndexes = new List<int>();
    private bool isOpen = false;
    public bool isShow = false;
    private bool canClick = true;

    public Sprite bill;
    public Sprite sand;
    public TimeCount time;
    private dialogeStory dialogue;
    public GameObject dialogueBG;
    public TypewriterEffectUI typeDia;
    void Start()
    {
        dialogue = GameObject.FindObjectOfType<dialogeStory>();
        
        isShow = true;
        type = GameObject.Find("Letter").GetComponent<TypewriterEffectUI>();
        player = FindObjectOfType<PlayerStatus>();
        mouse = FindObjectOfType<MouseTexture>();
        room = FindObjectOfType<ChangeRoom>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if ((room.roomInt != 2 || shownIndexes.Count >= letter.Length || time.dayCount >= 7) || player.isDream)
        {
            transform.position = new Vector3(-9.2f, -12.85f, 0);
        }
        else
        {
            if (isShow)
                transform.position = new Vector3(-1.84f, -2.57f, 0);
        }

        

    }

    public void SkipAndClose()
    {
        if(isOpen)
        {

            // ถ้ากำลังพิมพ์ → Skip ก่อน
            if (type != null && type.IsTyping())
            {
                type.SkipText();
                return;
            }
            CloseLetter();

        }
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseDown()
    {

        GameObject check = GameObject.Find("DialogeStoryBG");
        if ((type != null && type.IsTyping()) || (check != null && check.activeSelf == true)) return;
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
        if (!canClick) return;

        AudioSource audioSource = GameObject.Find("letterSound").GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
        if (!isOpen)
        {
            OpenLetter(transform.GetChild(0));
        }
        else
        {
            CloseLetter();
        }

        StartCoroutine(ClickCooldown());
    }

    IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(0.25f);
        canClick = true;
    }

    private void OpenLetter(Transform child0)
    {
        if (child0 == null) return;

        // สุ่มข้อความแบบไม่ซ้ำ
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < letter.Length; i++)
            if (!shownIndexes.Contains(i))
                availableIndexes.Add(i);

        if (availableIndexes.Count == 0)
        {
            CloseLetter(); // ถ้าไม่มีข้อความเหลือแล้ว → ปิด Letter
            return;
        }

        int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        shownIndexes.Add(randomIndex);

        child0.gameObject.SetActive(true);
        isOpen = true;
        isShow = false;


        if (sr != null)
            transform.position *= 5;

        // แสดงข้อความ
        if (child0.childCount > 0)
        {
            Text textComp = child0.GetChild(1).GetComponent<Text>();
            type.textComponent = textComp;
            type.ShowText(letter[randomIndex]);

            // แสดงรูปตาม index
            Transform imageChild = child0.GetChild(2);
            if (randomIndex == 2)
            {
                imageChild.gameObject.SetActive(true);
                imageChild.GetComponent<Image>().sprite = bill;
                imageChild.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 140f);
            }
            else if (randomIndex == 3)
            {
                imageChild.gameObject.SetActive(true);
                imageChild.GetComponent<Image>().sprite = sand;
                imageChild.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 100f);
            }
            else
            {
                imageChild.gameObject.SetActive(false);
            }
        }
    }
    public IEnumerator ShowLetterDialogueByDay(int day)
    {
        if (dialogue == null || typeDia == null)
        {
            Debug.LogWarning("dialogeStory หรือ TypewriterEffectUI ยังไม่ถูกเซ็ต");
            yield break;
        }

        // ✅ ดึงบทพูด stage 5 ของวันนั้น
        List<MyData> letterDialogs = dialogue.GetDialogsByDayAndStage(day, 5);

        // ถ้าไม่มีบทพูด → ออกจาก coroutine ทันที (ไม่ต้อง active BG)
        if (letterDialogs == null || letterDialogs.Count == 0)
        {
            Debug.Log($"ไม่มีบทพูดจดหมายสำหรับวันที่ {day}");
            yield break;
        }

        // ✅ มีบทพูด → ค่อยเปิดพื้นหลัง
        if (dialogueBG != null)
            dialogueBG.SetActive(true);

        for (int i = 0; i < letterDialogs.Count; i++)
        {
            var line = letterDialogs[i];

            // พิมพ์ข้อความทีละตัว
            typeDia.ShowText(line.dialoge);

            // รอจนกว่าพิมพ์เสร็จ
            yield return new WaitUntil(() => !typeDia.IsTyping());

            // หน่วงเวลานิดหนึ่งก่อนแสดงบรรทัดต่อไป
            yield return new WaitForSeconds(1f);
        }

        // ✅ ปิดพื้นหลังเมื่อจบ
        if (dialogueBG != null)
            dialogueBG.SetActive(false);
    }


    public void CloseLetter()
    {


        if (transform.childCount == 0) return;

        StartCoroutine(ShowLetterDialogueByDay(time.dayCount));

        Transform child0 = transform.GetChild(0);
        if (child0.gameObject.activeSelf)
        {
            child0.gameObject.SetActive(false);
            if (type != null)
                type.SkipText(); // ป้องกันข้อความค้าง

            isOpen = false;
        }
    }
}
