using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSown : MonoBehaviour
{
    public AudioSource winSound;
    // Start is called before the first frame update
    void Start()
    {
        winSound.PlayOneShot(winSound.clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
