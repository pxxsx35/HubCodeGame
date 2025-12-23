using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class evelator : MonoBehaviour
{
    public AudioSource evelatorSound;
    playerControl player;
    [SerializeField] Vector3 up, down;
    bool canUse;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<playerControl>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) && canUse == true && up != Vector3.zero)
        {
            player.allowMove = false;
            player.transform.position = up;
            evelatorSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.S) && canUse == true && down != Vector3.zero)
        {
            player.allowMove = false;
            player.transform.position = down;
            evelatorSound.Play();

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            canUse = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);

            canUse = false;
            player.allowMove = true;
             
            

        }
    }


}
