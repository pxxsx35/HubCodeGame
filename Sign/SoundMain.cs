using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMain : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource sound;
    void Start()
    {
        sound.PlayOneShot(sound.clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
