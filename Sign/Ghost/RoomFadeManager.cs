using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomFadeManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] room; // ถ้าเป็น SpriteRenderer
    [SerializeField] private float fadeDuration = 2f; // ความเร็วในการจาง
    [SerializeField] private string nextSceneName = "Win"; // Scene ปลายทาง

    public void StartFadeOutAllRooms()
    {
        StartCoroutine(FadeOutRoomsAndLoadScene());
    }

    private IEnumerator FadeOutRoomsAndLoadScene()
    {
        float elapsed = 0f;

        // เก็บสีต้นฉบับไว้ก่อน
        Color[] originalColors = new Color[room.Length];
        for (int i = 0; i < room.Length; i++)
        {
            if (room[i] != null)
                originalColors[i] = room[i].color;
        }

        // จางทุก sprite พร้อมกัน
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            for (int i = 0; i < room.Length; i++)
            {
                if (room[i] != null)
                {
                    Color c = originalColors[i];
                    c.a = alpha;
                    room[i].color = c;
                }
            }

            yield return null;
        }

        // ให้มั่นใจว่าทุกอันเป็น 0
        for (int i = 0; i < room.Length; i++)
        {
            if (room[i] != null)
            {
                Color c = room[i].color;
                c.a = 0f;
                room[i].color = c;
            }
        }

        // รอแป๊บหนึ่ง (optional)
        yield return new WaitForSeconds(1f);

        // โหลด Scene Win
        SceneManager.LoadScene(nextSceneName);
    }
}
