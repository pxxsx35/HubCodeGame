using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MyData
{
    public string dialoge;
    public int stage;
    public int day;

    public MyData(string dialoge, int stage, int day)
    {
        this.dialoge = dialoge;
        this.stage = stage;
        this.day = day;
    }
}

public class dialogeStory : MonoBehaviour
{
    [SerializeField]
    public List<MyData> dialogs = new List<MyData>();

    public Text text;
    private TimeCount time;
    private TypewriterEffectUI type;
    public GameObject dialogueBG;

    private static HashSet<int> shownStage4Days = new HashSet<int>(); // เก็บวันที่โชว์แล้ว

    void Start()
    {
        type = FindObjectOfType<TypewriterEffectUI>();
        time = FindObjectOfType<TimeCount>();

        // ====== DATA เดิม ======
        dialogs.Add(new MyData("แปลกจัง จำได้ว่าฉันกำลังนั่งอยู่บนรถประจำทางหลังกลับมาจากที่ทำงานพิเศษนี่นา,", 1, 1));
        dialogs.Add(new MyData("ทำไมถึงได้มาโผล่ในที่แบบนี้ล่ะ?,", 1, 1));
        dialogs.Add(new MyData("ที่นี่…น่ากลัวจัง ฉันต้องหาทางออกไป,", 1, 1));

        dialogs.Add(new MyData("ของแบบนี้มาอยู่ในที่แบบนี้เนี่ยนะ?,", 2, 1));
        dialogs.Add(new MyData("บางทีอาจจะมีใครวางของพวกนี้เอาไว้เพื่ออะไรบางอย่าง,", 2, 1));

        dialogs.Add(new MyData("!!!,", 3, 1));
        dialogs.Add(new MyData("ฉันตื่นขึ้นมากลางดึกหรือว่ากำลังฝันอยู่กันแน่? แต่ไม่ว่าแบบไหนฉันก็พูดอะไรไม่ออกเลย,", 3, 1));
        dialogs.Add(new MyData("มีใครบางคนยืนอยู่ตรงหน้าฉัน,", 3, 1));
        dialogs.Add(new MyData("ลำคอของฉันแห้งผาก แม้มีคำมากมายที่อยากจะพูดออกไป แต่เสียงที่เปล่งออกมานั้นมีเพียงแค่,", 3, 1));

        // ====== DAY 2 ======
        dialogs.Add(new MyData("ฉันมองออกไปนอกหน้าต่างแต่ไม่เห็นอะไรเลย มีประตูหลายบานแต่ฉันไม่รู้ว่างทางออกอยู่ตรงไหน,", 1, 2));
        dialogs.Add(new MyData("ฉันออกไปจากบ้านหลังนี้ไม่ได้,", 1, 2));

        dialogs.Add(new MyData("ฉันเจอจดหมายวางอยู่ในบ้าน เปลี่ยนที่ไปทุกวัน ฉันไม่รู้ว่าเจ้าของจดหมายฉบับนี้เขียนถึงใคร ไม่มีทั้งชื่อผู้ส่งและผู้รับ,", 5, 2));
        dialogs.Add(new MyData("ฉันเดาว่าคนเขียนอาจจะเป็นเจ้าของบ้าน แต่นั่นก็ไม่สำคัญหรอก ฉันไม่จำเป็นต้องรู้อะไรเกี่ยวกับบ้านหลังนี้,", 5, 2));

        dialogs.Add(new MyData("ฉันเจอของพวกนี้อีกแล้ว สงสัยจังว่าใครกันแน่ที่เป็นคนเอามาวางเอาไว้ บางทีอาจจะเกี่ยวข้องกับคนที่ฉันเจอเมื่อคืนก็ได้,", 2, 2));
        dialogs.Add(new MyData("ฉัน…เริ่มจะแยกความจริงกับความฝันไม่ออกแล้ว,", 2, 2));

        dialogs.Add(new MyData("วันนี้จะมีคนขอมาอยู่ด้วยอีกไหมนะ? อ้ะ แต่ฉันก็ไม่แน่ใจหรอกว่าพวกนั้นเป็นคนหรือเปล่า,", 3, 2));

        // ====== DAY 3 ======
        dialogs.Add(new MyData("ฉันมาอยู่ที่นี่ได้กี่วันแล้วนะ? เริ่มที่จะคุ้นเคยกับบ้านที่ไม่มีอะไรเลยหลังนี้แล้วสิ ไม่รู้ว่ามันเป็นเรื่องดีหรือเรื่องร้ายกันแน่,", 1, 3));
        dialogs.Add(new MyData("เกลียดการอยู่คนเดียวจัง…,", 1, 3));
        dialogs.Add(new MyData("ฉันอ่านจดหมายที่วางไว้แทบทุกวัน ฉันไม่ได้อยากรู้เรื่องราวในนั้นหรอก เพียงแค่ฉันไม่รู้ว่าจะทำอะไรในบ้านหลังนี้แล้วก็เท่านั้นเอง,", 5, 3));

        // ====== DAY 4 ======
        dialogs.Add(new MyData("เหนื่อยจัง ฉันอยากนอนต่ออีกสักหน่อย ถึงมันจะเช้าแล้วก็เถอะ,", 1, 4));
        dialogs.Add(new MyData("เจ้าของบ้านหลังนี้คงมีเรื่องมากมายที่อยากจะระบายเลยเขียนมันออกมา,", 5, 4));
        dialogs.Add(new MyData("จะว่าไปแล้วฉันเคยมีเรื่องทุกข์ใจแบบนี้บ้างไหมนะ? จำไม่ได้เลย,", 5, 4));
        dialogs.Add(new MyData("ไม่สิ…ฉันจำแม้แต่ชื่อของตัวเองไม่ได้ด้วยซ้ำ,", 5, 4));

        // ====== DAY 5 ======
        dialogs.Add(new MyData("ฉันต้อนรับสิ่งพวกนั้นเข้ามาในบ้านทุกคืน ห้องดูคับแคบขึ้น แต่ฉันก็ยังรู้สึกว่ามันยังกว้างเกินไปอยู่ดี,", 1, 5));
        dialogs.Add(new MyData("ความโดดเดี่ยวที่กัดกินจิตใจมันทำให้ฉันแทบบ้า เมื่อไหร่จะจบเสียที ฉันต้องอยู่แบบนี้ไปอีกนานแค่ไหน?,", 1, 5));

        // ====== DAY 6 ======
        dialogs.Add(new MyData("…,", 1, 6));

        // ====== DAY 7 ======
        dialogs.Add(new MyData("…ถ้าเรื่องทั้งหมดจบลงตรงนี้ก็คงดี บางทีฉันคงต้องลองสวดอ้อนวอนดูบ้าง อาจจะมีใครสักคนที่ตอบรับคำขอของฉัน,", 1, 7));
        dialogs.Add(new MyData("เหมือนว่าฉันจะรู้สึกคุ้นเคย แต่ฉันไม่แน่ใจ", 4, 1));
    }

    // ==========================================
    // 🔹 ฟังก์ชันเดิม
    // ==========================================
    public List<MyData> GetDialogsByDay(int day)
    {
        return dialogs.Where(d => d.day == day).ToList();
    }

    public List<MyData> GetDialogsByStage(int stage)
    {
        if (time == null)
            time = FindObjectOfType<TimeCount>();

        int currentDay = time != null ? time.dayTime : 1;
        return dialogs.Where(d => d.stage == stage && d.day == currentDay).ToList();
    }

    public void ShowDialogByStage(int stage)
    {
        var list = GetDialogsByStage(stage);
        if (list.Count > 0)
        {
            if (type != null)
                type.ShowText(list[0].dialoge);
            else if (text != null)
                text.text = list[0].dialoge;
            else
                Debug.Log(list[0].dialoge);
        }
        else
        {
            Debug.LogWarning($"No dialog found for stage {stage}");
        }
    }

    public List<MyData> GetDialogsByDayAndStage(int day, int stage)
    {
        return dialogs.Where(d => d.day == day && d.stage == stage).ToList();
    }

    // ==========================================
    // 🔹 ฟังก์ชันใหม่: Stage 4 Dialogue
    // ==========================================
    public IEnumerator ShowStage4Dialogue(int day)
    {
        if (type == null)
            type = FindObjectOfType<TypewriterEffectUI>();

        if (shownStage4Days.Contains(day))
        {
            Debug.Log($"Stage 4 dialogue สำหรับวันที่ {day} แสดงไปแล้ว");
            yield break;
        }

        List<MyData> stage4Dialogs = GetDialogsByDayAndStage(day, 2);
        if (stage4Dialogs == null || stage4Dialogs.Count == 0)
        {
            Debug.Log($"ไม่มี Stage 2 dialogue สำหรับวันที่ {day}");
            yield break;
        }

        if (dialogueBG != null)
            dialogueBG.SetActive(true);

        foreach (var line in stage4Dialogs)
        {
            type.ShowText(line.dialoge);
            yield return new WaitUntil(() => !type.IsTyping());
            yield return new WaitForSeconds(1f);
        }

        if (dialogueBG != null)
            dialogueBG.SetActive(false);

        shownStage4Days.Add(day);
    }
}
