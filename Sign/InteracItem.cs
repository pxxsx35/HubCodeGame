using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteracItem : MonoBehaviour
{
    private Camera cam;
    private PlayerStatus player;
    private MouseTexture mouse;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {

        cam = Camera.main;
        mouse = FindObjectOfType<MouseTexture>();
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
        player = FindAnyObjectByType<PlayerStatus>();
        spriteRenderer = GetComponent<SpriteRenderer>();
}

    private void Update()
    {


    }
    private void OnMouseDown()
    {
        TypewriterEffectUI type = GameObject.Find("DialogeStory").GetComponent<TypewriterEffectUI>();
        GameObject check = GameObject.Find("DialogeStoryBG");
        if ((type != null && type.IsTyping()) || (check != null && check.activeSelf == true)) return;
        if (gameObject.CompareTag("bed") && !player.isDream)
        {
            StartCoroutine(player.ConfirmSleep());
           
            Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);

        }


    } 
    private void OnMouseEnter()
    {
        if(gameObject.CompareTag("bed") && !player.isDream)
        {
            Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
        }
    }   
    private void OnMouseExit()
    {
        if(gameObject.CompareTag("bed") && !player.isDream)
        {
            Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
        }
    }



}
