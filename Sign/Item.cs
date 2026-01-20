using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Typewriter Effect สำหรับแสดงชื่อไอเทม")]
    public TypewriterEffectUI typewriter;

    [Header("ข้อมูลไอเทม")]
    public string itemName;
    public bool isOpen;
    public int displayType; // 1 = หันตรง, 2 = หันข้าง

    [Header("Sprite สำหรับแสดงผล")]
    public Sprite itemSprite;

    [Header("Room System (ตั้งค่าอัตโนมัติ)")]
    [SerializeField] private int roomNumber;
    [SerializeField] private int assignedDay;
    public ChangeRoom roomController;
    public TimeCount timeCounter;

    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    private int currentDisplayRoom = -1;
    private int currentDay = -1;

    private Image img;
    private Image bgImg;
    private PopupController popupController;
    private MouseTexture mouse;

    public float speed = 2f;
    public float angle = 30f;

    private bool isRotating = false;
    private dialogeStory dialogue;

    void Start()
    {
        // ดึงอ็อบเจ็กต์หลักต่าง ๆ
        dialogue = FindObjectOfType<dialogeStory>();
        typewriter = GameObject.Find("EffectItemName").GetComponent<TypewriterEffectUI>();
        popupController = FindObjectOfType<PopupController>();
        mouse = FindObjectOfType<MouseTexture>();
        img = GameObject.Find("ItemShow").GetComponent<Image>();
        bgImg = GameObject.Find("ItemPopup").GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();

        transform.localScale = Vector3.one / 3.5f;

        // หา Controller อัตโนมัติถ้ายังไม่ได้เซ็ต
        if (roomController == null)
            roomController = FindObjectOfType<ChangeRoom>();
        if (timeCounter == null)
            timeCounter = FindObjectOfType<TimeCount>();

        if (spriteRenderer != null)
            SetDisplayType(displayType);

        // เซ็ตค่าเริ่มต้น
        if (roomController != null && timeCounter != null)
        {
            currentDisplayRoom = roomController.roomInt;
            currentDay = timeCounter.dayCount;
            UpdateVisibility();
        }
    }

    void Update()
    {
        if (roomController != null && timeCounter != null)
        {
            bool roomChanged = (roomController.roomInt != currentDisplayRoom);
            bool dayChanged = (timeCounter.dayCount != currentDay);

            if (roomChanged || dayChanged)
            {
                currentDisplayRoom = roomController.roomInt;
                currentDay = timeCounter.dayCount;
                UpdateVisibility();
            }

            if (isRotating)
                RotateXBackAndForth(speed, angle);
        }
    }

    void UpdateVisibility()
    {
        if (spriteRenderer != null)
        {
            bool inCorrectRoom = (currentDisplayRoom == roomNumber);
            bool inCorrectDay = (currentDay == assignedDay);
            bool shouldShow = inCorrectRoom && inCorrectDay;

            spriteRenderer.enabled = shouldShow;

            if (itemCollider != null)
                itemCollider.enabled = shouldShow;
        }
    }

    public void SetDisplayType(int type)
    {
        displayType = type;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning($"⚠️ {itemName} ไม่มี SpriteRenderer!");
            return;
        }

        if (itemSprite != null)
            spriteRenderer.sprite = itemSprite;
        else
            Debug.LogWarning($"⚠️ {itemName} ไม่มี Sprite!");
    }

    public void SetRoom(int room)
    {
        roomNumber = room;
        UpdateVisibility();
    }

    public void SetDay(int day)
    {
        assignedDay = day;
        UpdateVisibility();
    }

    public int GetRoom() => roomNumber;
    public int GetDay() => assignedDay;

    private void OnMouseEnter()
    {
        isRotating = true;
        Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        isRotating = false;
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseDown()
    {
        // ป้องกันพิมพ์ตัวหนังสืออยู่แล้ว
        if (typewriter.IsTyping() || (dialogue != null && dialogue.dialogueBG.activeSelf)) return;

        bool inCorrectRoom = (roomController.roomInt == roomNumber);
        bool inCorrectDay = (timeCounter.dayCount == assignedDay);
        if (popupController == null || popupController.isPopupOpen) return;
        if (!inCorrectRoom || !inCorrectDay) return;

        Debug.Log($"🖱️ คลิกไอเทม: {itemName}");
        AudioSource audioSource = GameObject.Find("pick").GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
        popupController.OpenPopup(img, bgImg);
        img.sprite = spriteRenderer.sprite;
        typewriter.ShowText(itemName);
        isOpen = true;

        // เรียก Stage 4 dialogue
        if (dialogue != null)
        {
            StartCoroutine(ShowStage4AndDestroy());
        }
    }

    private IEnumerator ShowStage4AndDestroy()
    {
        // เรียก coroutine ของ dialogueStory
        yield return dialogue.StartCoroutine(dialogue.ShowStage4Dialogue(timeCounter.dayCount));

        // หลังจบ dialogue → ลบไอเทม
        Destroy(gameObject);
    }

    void RotateXBackAndForth(float speed, float angle)
    {
        float rotationZ = Mathf.Sin(Time.time * speed) * angle;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rotationZ);
    }
}
