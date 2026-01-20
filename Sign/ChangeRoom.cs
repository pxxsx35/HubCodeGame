    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public int roomInt; //1) Bed room 2)living room  3)kitchen
    public PlayerStatus player;
    public TimeCount time;
    public GameObject dialoge;
    private void Start()
    {
        roomInt = 2;
    }

    private void Update()
    {
        RoomDisplay();
        if (!player.isDream)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                Previous();
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                NextRoom();
            }
        }

        if (player.isDream)
            roomInt = player.roomInt + 1;

       
        
    }

    void RoomDisplay()
    {
        

        gameObject.transform.GetChild(roomInt - 1).gameObject.SetActive(true);

        if (roomInt != 1)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (roomInt != 2)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (roomInt != 3)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (roomInt != 4)
        {
            gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }

    }

    public void NextRoom()
    {
        if (!player.isDream && !player.isConfirm && dialoge.activeSelf == false)
        {
            if (roomInt == 1)
            {
                roomInt = 2;
            }
            else if (roomInt == 2)
            {
                roomInt = 3;
            }
            else if (roomInt == 3)
            {
                roomInt = 4;
            }
        }
    }   
    public void Previous()
    {
        if (!player.isDream && !player.isConfirm &&  dialoge.activeSelf == false)
        {
            if (roomInt == 2)
            {
                roomInt = 1;
            }
            else if (roomInt == 3)
            {
                roomInt = 2;
            }
            else if (roomInt == 4)
            {
                roomInt = 3;
            }
        }
    }

}
