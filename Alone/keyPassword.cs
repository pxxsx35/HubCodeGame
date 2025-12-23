using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyPassword : MonoBehaviour
{
    private bool canOpenQuest = false;

    private playerControl player;
  

    void Start()
    {

        player = FindAnyObjectByType<playerControl>();
      

    }

    void Update()
    {
        if (canOpenQuest && Input.GetKeyDown(KeyCode.E))
        {

            ToggleQuestUI();


        }
    }

    void ToggleQuestUI()
    {
        bool isActive = !transform.GetChild(1).gameObject.activeSelf;
        transform.GetChild(1).gameObject.SetActive(isActive);

        if (isActive)
        {
           


            player.allowMove = false;

        }
        else
        {
            
            player.allowMove = true;
        }


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            canOpenQuest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            player.allowMove = true;
            canOpenQuest = false;
        }
    }


}