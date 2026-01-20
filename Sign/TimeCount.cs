using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    private Text text;
    public PlayerStatus player;
    public ChangeRoom room;
    public bool multiSpeed;
    public Text textDay;
    public float timeInSeconds = 21600;   //21600 = 6 H   79,200 = 22 H
    private float timeMultiplier;
    private int minuteToSecond = 60 * 60;
    public int dayCount = 1;
    public int dayTime;
    public int nightTime;
    public float dayCycle;  //1 Day = ? minutes For Day.
    public float nightCycle;  //1 Day = ? minutes For Night.




    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
   
    }

    // Update is called once per frame
    void Update()
    {

        System.TimeSpan time = System.TimeSpan.FromSeconds(timeInSeconds);
        if (time.Hours >= 22 || time.Hours < 6)
        {
            timeMultiplier = ((24 - (nightTime - dayTime)) * minuteToSecond) / (nightCycle * 60);
            player.isDream = true;
         
        }
        else
        {
            timeMultiplier = ((nightTime - dayTime) * minuteToSecond) / (dayCycle * 60);
            player.isDream = false;
   

        }


        string formatted = string.Format("{0:D2}:{1:D2}", time.Hours, time.Minutes);
        text.text = formatted;
        dayCount = (int)((timeInSeconds - (dayTime * minuteToSecond)) / (24 * 60 * 60)) + 1;

        textDay.text = " " + dayCount;
        timeCount();

        if((time.Hours == 6 && time.Minutes < 5) && dayCount != 1)
        {
            room.roomInt = 1;
        }
    

    }

    public void timeCount()
    {
      if(multiSpeed)
        timeInSeconds += Time.deltaTime * timeMultiplier * 30;
      else
        timeInSeconds += Time.deltaTime * timeMultiplier;


    }
}
