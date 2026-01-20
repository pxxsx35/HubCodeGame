using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class useSalang : MonoBehaviour
{
    // Start is called before the first frame update
    public Image salang;
    public AudioSource salangSound;
    private PlayerStatus player;
    private MouseTexture mouse;


    private void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        mouse = FindObjectOfType<MouseTexture>();
    }

    private void OnMouseDown()
    {
        if(player.isSalang)
        {
            player.canOpen = true;
            player.isSalang = false;
            Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
            player.salangUse.gameObject.SetActive(false);
            Destroy(gameObject);
            salangSound.PlayOneShot(salangSound.clip);
        }
    }
    private void OnMouseEnter()
    {
        if(player.isSalang)
        Cursor.SetCursor(mouse.Salang, Vector2.zero , CursorMode.Auto);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);
    }
}
