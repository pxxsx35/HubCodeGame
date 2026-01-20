using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform player;
    public float maxVolume = 1f;
    public float minDistance = 1f;
    public float maxDistance = 10f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        float volume = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));

        audioSource.volume = volume * maxVolume;
    }
}
