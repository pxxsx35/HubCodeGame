using UnityEngine;
using System.Collections;

public class enemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int maxEnemies = 10;    
    public Vector2 spawnPosition; 
    private bool hasTriggered = false; 
    public float spawnDelay = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; 
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
         
            Vector2 newPosition = spawnPosition + new Vector2(Random.Range(-10f, 10f) , 0f);
            Instantiate(enemyPrefab, newPosition, Quaternion.identity);

         
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
