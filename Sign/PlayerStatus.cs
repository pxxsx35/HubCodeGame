using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public GameObject dialogueBG;
    public TypewriterEffectUI textDia;
    public GameObject confirm;
    public TimeCount time;
    public bool isDream;
    public int roomInt;
    public int damage;
    public int randomRoom;
    public Image fearBar;
    public float fear;
    public float maxFear;
    private bool changeRoom;
    public bool isDead;
    private GameObject bedroom;
    private MouseTexture mouse;
    public float timePray;
    public float maxTimePray;
    public bool isPray;
    public bool isSalang;
    public bool isConfirm = false;
    public GameObject letter;
    public GameObject soad;
    public GameObject salang;
    [SerializeField] private GameObject gameOverUI;
    public bool canOpen;
    private dialogeStory dialoge;
    public AudioSource gameOverSound;
    public AudioSource clickSound;
    public GameObject allUI;
    public Image pray;
    public Image salangUse;
    public AudioSource bathroomsound;
    public GameObject bathroom;
    public AudioSource salangSound;
    public AudioSource SoadSound;
    public AudioSource soundMorning;
    public AudioSource soundNight;

    void Start()
    {
        dialoge = FindObjectOfType<dialogeStory>();

        // ตั้ง loop ก่อนเล่นเสียง
        soundMorning.loop = true;
        soundMorning.Play();

        soundNight.loop = true;

        isConfirm = false;
        isSalang = false;
        bedroom = GameObject.Find("r1");
        timePray = maxTimePray;
        mouse = FindObjectOfType<MouseTexture>();
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
        fear = maxFear / 2;

        if (!isDead) soad.SetActive(false);
        if (time.dayCount != 8) salang.SetActive(false);

        StartCoroutine(InitMorningDialogue());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickSound.PlayOneShot(clickSound.clip);
        }


        if (bathroom.activeSelf)
            bathroomsound.PlayOneShot(bathroomsound.clip);

        UpdateFear();

        if (time.dayCount == 8)
            salang.SetActive(true);

        if (isDream && !dialogueBG.activeSelf)
            fear += (Time.deltaTime / 3);

        if (!isDream)
            changeRoom = false;

        roomInt = randomRoomInt();

        if (isPray)
        {
            timePray -= Time.deltaTime;
            if (timePray <= 0)
            {
                fear -= 5;
                isPray = false;
                pray.gameObject.SetActive(false);
                soad.gameObject.SetActive(false);
                timePray = maxTimePray;
            }
        }

        if (isDead)
            soad.gameObject.SetActive(true);

        if (fear >= maxFear)
        {
            gameOverUI.SetActive(true);
            allUI.SetActive(false);
            gameOverSound.PlayOneShot(gameOverSound.clip);
            WakeUp();
        }
    }

    IEnumerator InitMorningDialogue()
    {
        yield return new WaitUntil(() => FindObjectOfType<dialogeStory>() != null);
        dialoge = FindObjectOfType<dialogeStory>();
        StartCoroutine(ShowMorningDialogue(time.dayCount));
    }

    IEnumerator ShowNightDialogue(int day)
    {
        if (dialoge == null) yield break;

        List<MyData> nightDialogs = dialoge.GetDialogsByDayAndStage(day, 3);
        if (nightDialogs.Count == 0)
        {
            Debug.Log($"ไม่มีบทพูดกลางคืนสำหรับวันที่ {day}");
            yield break;
        }

        dialogueBG.SetActive(true);
        for (int i = 0; i < nightDialogs.Count; i++)
        {
            textDia.ShowText(nightDialogs[i].dialoge);
            yield return new WaitUntil(() => !textDia.IsTyping());
            yield return new WaitForSeconds(1f);
            if (i == nightDialogs.Count - 1)
                dialogueBG.SetActive(false);
        }
    }

    int randomRoomInt()
    {
        if (isDream && !changeRoom)
        {
            do
            {
                randomRoom = Random.Range(0, 3);
            } while (randomRoom == roomInt);

            roomInt = randomRoom;
            changeRoom = true;
        }
        return randomRoom;
    }

    void UpdateFear()
    {
        fearBar.fillAmount = fear / maxFear;
        if (fear <= 0) fear = 0;
        if (fear >= 20) Debug.Log("Lose");
    }

    public void Sleep()
    {
        int timeHour = (int)(((time.timeInSeconds + (24 * 3600)) % 86400) / 3600);
        GameObject.FindObjectOfType<ItemManager>().ClearCurrentDayItems();

        if (!isDream)
        {
            int timeCount = 22 - Mathf.Abs(timeHour);
            time.timeInSeconds += (timeCount * 3600);
        }

        StartCoroutine(ShowNightDialogue(time.dayCount));

        // สลับเสียง
        if (soundMorning.isPlaying) soundMorning.Stop();
        soundNight.loop = true;
        soundNight.Play();
    }

    public void WakeUp()
    {
        if (isDream)
        {
            int timeHour = (int)((time.timeInSeconds % 86400) / 3600);
            int targetHour = 6;
            int timeCount = targetHour - timeHour;
            if (timeCount <= 0) timeCount += 24;

            if (soundNight.isPlaying) soundNight.Stop();
            soundMorning.loop = true;
            soundMorning.Play();

            time.timeInSeconds += timeCount * 3600;
            GameObject.FindObjectOfType<ChangeRoom>().roomInt = 1;
            letter.transform.position /= 5;
            letter.GetComponent<Letter>().isShow = true;

            if (fear < maxFear)
                StartCoroutine(ShowMorningDialogue(time.dayCount + 1));
        }
    }

    IEnumerator ShowMorningDialogue(int day)
    {
        if (dialoge == null) yield break;

        List<MyData> morningDialogs = dialoge.GetDialogsByDayAndStage(day, 1);
        if (morningDialogs.Count == 0)
        {
            Debug.Log($"ไม่มีบทพูดสำหรับวันที่ {day}");
            yield break;
        }

        dialogueBG.SetActive(true);
        for (int i = 0; i < morningDialogs.Count; i++)
        {
            textDia.ShowText(morningDialogs[i].dialoge);
            yield return new WaitUntil(() => !textDia.IsTyping());
            yield return new WaitForSeconds(1f);
            if (i == morningDialogs.Count - 1)
                dialogueBG.SetActive(false);
        }
    }

    public void usePray()
    {
        if (isDead)
        {
            soad.SetActive(false);
            isDead = false;
            pray.gameObject.SetActive(true);
            isPray = true;
            Sleep();
            SoadSound.Play();
        }
    }

    public void useSalang()
    {
        salangSound.Play();
        salangUse.gameObject.SetActive(true);
        isSalang = true;
    }

    public IEnumerator ConfirmSleep()
    {
        isConfirm = true;
        yield return new WaitForSeconds(0.1f);
        confirm.SetActive(true);
    }

    public void SureSleep()
    {
        Sleep();
        isConfirm = false;
        confirm.SetActive(false);
    }

    public void NotSleep()
    {
        isConfirm = false;
        confirm.SetActive(false);
    }
}
