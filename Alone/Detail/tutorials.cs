using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorials : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(text);
            Destroy(gameObject);
        }
    }
}
