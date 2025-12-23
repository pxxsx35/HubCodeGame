using UnityEngine;
using UnityEngine.UI;

public class NumPad : MonoBehaviour
{
    public Text displayText; // อ้างอิงไปยัง TextMeshPro ที่จะแสดงผล
    private RandomKey randomKey;
    playerControl player;
    public GameObject wrong, right;
    PasswordFeedback feedback;
    public AudioSource[] checkSound;
    public AudioSource type;
    public GameObject door;

    void Start()
    {
        player = FindAnyObjectByType<playerControl>();
        feedback = FindObjectOfType<PasswordFeedback>(); // หา Script ที่ใช้แสดงผล
        
    }

    public void CheckBtn()
    {
        randomKey = FindObjectOfType<RandomKey>(); // หา instance ของ RandomKey

        for (int i = 1; i <= 9; i++)
        {
            int number = i;
            Button button = GameObject.Find("Button" + number).GetComponent<Button>();

            button.onClick.RemoveAllListeners(); // ล้าง Listeners เดิมก่อนเพิ่มใหม่
            button.onClick.AddListener(() => AppendNumber(number));
        }
    }


    void AppendNumber(int number)
    {
        type.Play();
        displayText.text += number.ToString(); // เพิ่มตัวเลขเข้าไปในข้อความเดิม

        if (displayText.text.Length >= 4)
        {
            Enter();
        }
    }

    void Enter()
    {
        if (displayText.text.Length < 4)
        {
            Debug.LogError("Input is less than 4 digits!");
            return;
        }

        int[] inputCode = new int[4];
        for (int i = 0; i < 4; i++)
        {
            inputCode[i] = int.Parse(displayText.text[i].ToString());
        }

        if (CheckPassword(inputCode))
        {
            Debug.Log("Password Correct!");
            feedback.ShowCorrect(); // แสดงรูปถูกต้อง
            displayText.text = "";
            gameObject.SetActive(false);
            checkSound[0].Play();
            player.allowMove = true;
            door.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Incorrect Password! Resetting...");
            feedback.ShowIncorrect(); // แสดงรูปผิด
            displayText.text = "";
            gameObject.SetActive(false);
            player.allowMove = true;
            checkSound[1].Play();


        }
    }


    void OnEnable()
    {
        print("Enable");
        displayText.text = "";
        CheckBtn(); // เรียกใหม่เมื่อเปิด UI
    }


    bool CheckPassword(int[] input)
    {
        for (int i = 0; i < 4; i++)
        {
            if (input[i] != randomKey.password[i])
            {
                return false;
            }
        }
        return true;
    }


}
