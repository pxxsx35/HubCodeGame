using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManage : MonoBehaviour
{
    public Sprite[] roomStage;
    private float fearPercent;
    private PlayerStatus player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerStatus>();   
    }

    // Update is called once per frame
    void Update()
    {
        fearPercent = (player.fear / player.maxFear) * 100;

        if(fearPercent < 25)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = roomStage[0];

            // Stage 0
        }
        else if(fearPercent < 75)
        {
            //Stage 1
            gameObject.GetComponent<SpriteRenderer>().sprite = roomStage[1];
        }else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = roomStage[2];

            //Stage 2

        }
    }
}
