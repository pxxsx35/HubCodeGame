using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnd : MonoBehaviour
{
    public AudioSource badSound;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<EndGame>().BadEnd();
        badSound.PlayOneShot(badSound.clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
