using UnityEngine;

public class MouseHoverAndClick2D : MonoBehaviour
{
    SpriteRenderer sprite;
    MouseTexture mouse;
    private bool isDisable;
    [SerializeField] float timeOpen;
    private float lasttimeOpen;
    private PlayerStatus player;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        mouse = FindObjectOfType<MouseTexture>();
        player = FindObjectOfType<PlayerStatus>();
        lasttimeOpen = timeOpen;
    }
    private void Update()
    {
        if(isDisable)
        {
            timeOpen -= Time.deltaTime;
            if(timeOpen <= 0)
            {
                isDisable = false;
            }
        }else
        {
            sprite.sortingOrder = 3;
            timeOpen = lasttimeOpen;
            transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
        }


    }

    private void OnMouseOver()
    {
        if(!player.isDream)
            Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
  

    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(mouse.mouse, Vector2.zero, CursorMode.Auto);

    }

    private void OnMouseUp()
    {
        if (!player.isDream)
        {
            sprite.sortingOrder = 0;
            isDisable = true;
            transform.localPosition = new Vector3(transform.position.x, transform.position.y, -10);
            Cursor.SetCursor(mouse.pointer, Vector2.zero, CursorMode.Auto);
        }

    }



}
