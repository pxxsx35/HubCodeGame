using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button startBtn;
    private PlayerStatus player;
    public void BtnMenu(string startScene)
    {
        SceneManager.LoadScene(startScene);
    }

    public void Restart(string startScene)
    {
        // สมัคร event ว่าพอ scene โหลดเสร็จให้เรียกฟังก์ชัน OnSceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(startScene);
    }

    // ฟังก์ชันนี้จะถูกเรียกหลังจาก scene โหลดเสร็จสมบูรณ์
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // หาวัตถุ GameOver แล้วปิด
        GameObject gameOver = GameObject.Find("GameOver");
        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }

        // หาผู้เล่นแล้วรีเซ็ตสถานะ
        player = FindObjectOfType<PlayerStatus>();
        if (player != null)
        {
            player.isDead = true; // ตั้งกลับเป็น false เพราะเรา restart แล้ว
        }

        // ยกเลิกการสมัคร event เพื่อไม่ให้ซ้ำตอน load รอบต่อไป
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        startBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        startBtn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}
