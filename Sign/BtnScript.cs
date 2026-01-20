using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnScript : MonoBehaviour
{
    // Start is called before the first frame update
    private MouseTexture mouse;
    private RoomManage room;
    void Start()
    {
        mouse = FindObjectOfType<MouseTexture>();
        room = FindObjectOfType<RoomManage>();
    }

    private void OnMouseEnter()
    {
        
    }
}
