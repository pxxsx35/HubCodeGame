using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpScare : MonoBehaviour
{
    float timeCount = 15f;
    bool start;
    public AudioSource jumpScares;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(start == true)
        {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0) 
            Destroy(gameObject);
        }

        if(timeCount <=5)
        {
            transform.GetChild(1).gameObject.SetActive(false);

            transform.position -= Vector3.up * 0.5f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            start = true;
            if(jumpScares != null)
                jumpScares.Play();
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(jumpScares);

        }
    }
}
