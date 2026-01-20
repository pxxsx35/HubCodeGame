using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private MouseTexture mouse;
    private PlayerStatus player;
    public GameObject bathroom;

    private void Start()
    {
        mouse = FindObjectOfType<MouseTexture>();
        player = FindObjectOfType<PlayerStatus>();
    }


    private void OnMouseDown()
    {
        if(player.canOpen)
        bathroom.SetActive(true);
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
    }
}
