using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public Text textComponent;  // ใช้ TextMeshPro
    public float typingSpeed = 0.05f;
    public AudioSource typeSound;
    public string[] fullTexts = new string[5]; // ข้อความของแต่ละเควส
    private string currentText = "";
    private bool canOpenQuest = false;
    private int currentQuestIndex = 0; // ติดตามเควสปัจจุบัน
    public bool[] questCompleted = new bool[5]; // ติดตามว่าเควสไหนสำเร็จแล้ว
    RandomKey key;
    private playerControl player;

    void Start()
    {

        player = FindAnyObjectByType<playerControl>();
        key = FindAnyObjectByType<RandomKey>();
    }

    void Update()
    {
        if (canOpenQuest && Input.GetKeyDown(KeyCode.E))
        {
        

            ToggleQuestUI();
        }
    }

    void ToggleQuestUI()
    {
        bool isActive = !transform.GetChild(1).gameObject.activeSelf;
        transform.GetChild(1).gameObject.SetActive(isActive);

        if (isActive)
        {
            if (currentQuestIndex < 5 && !questCompleted[currentQuestIndex])
            {
                if (textComponent.text == "")
                {
                    int textIndex = key.randomKey(currentQuestIndex); 
                    if (textIndex != -1 && textIndex < fullTexts.Length)
                    {
                        StartCoroutine(TypeText(fullTexts[textIndex])); 
                    }
                    else
                    {
                        Debug.LogWarning("Index ผิดพลาด: textIndex = " + textIndex);
                        StartCoroutine(TypeText(fullTexts[4]));

                    }
                }
            }

            player.allowMove = false;
        }
        else
        {
            textComponent.text = "";
            player.allowMove = true;
        }
    }


    public void CompleteQuest()
    {
        if (currentQuestIndex < 5)
        {
            questCompleted[currentQuestIndex] = true;
            currentQuestIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            canOpenQuest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            player.allowMove = true;

            canOpenQuest = false;
        }
    }

    IEnumerator TypeText(string text)
    {
        if (textComponent == null)
        {
            Debug.LogError("textComponent ไม่ถูกกำหนดค่า! โปรดเช็คใน Inspector");
            yield break;
        }

        if (string.IsNullOrEmpty(text))
        {
            Debug.LogError("ข้อความของเควสว่างเปล่าหรือไม่ได้ถูกกำหนดค่า!");
            yield break;
        }

        currentText = "";
        for (int i = 0; i <= text.Length; i++)
        {
            typeSound.Play();
            currentText = text.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

}
