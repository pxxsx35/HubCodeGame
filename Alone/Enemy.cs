using UnityEngine;
using System.Collections;


public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;
    public float patrolDistance = 50f;
    public float chaseRange = 7f;
    public float attackRange = 10f;
    public float attackDelay = 0.5f;
    public float knockbackForce = 5f;

    private bool movingRight = false;
    private Vector2 startPos;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private playerControl playerControl;
    private float timeInAttackRange = 0f;
    public GameObject bite;

    void Start()
    {
        bite = GameObject.Find("bite");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerControl = player?.GetComponent<playerControl>();

        if (player == null || playerControl == null)
        {
            Debug.LogWarning("Player or PlayerControl not found!");
        }
    }

    void Update()
    {
        if (playerControl.dead) return;
        if (player == null || playerControl == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            timeInAttackRange += Time.deltaTime;
            if (timeInAttackRange >= attackDelay)
            {
                StopAndAttackPlayer();
                timeInAttackRange = 0f;
            }
        }
        else
        {
            timeInAttackRange = 0f;
        }

        if (distanceToPlayer <= chaseRange)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }

      
    }

    void Patrol()
    {
        float moveDirection = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

        if (movingRight && transform.position.x >= startPos.x + patrolDistance)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= startPos.x - patrolDistance)
        {
            Flip();
        }

        animator.SetBool("isWalking", true);
    }

    void FollowPlayer()
    {
        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
        {
            Flip();
        }

        animator.SetBool("isWalking", true);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
    }

    void StopAndAttackPlayer()
    {
        rb.velocity = Vector2.zero; // หยุดการเคลื่อนที่
        animator.SetTrigger("atk");
        bite.GetComponent<AudioSource>().Play();

        StartCoroutine(AttackPlayer());
    }

    IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Enemy Attacks Player!");
            playerControl.healthPoint -= 25;
            playerControl.animator.SetTrigger("hurt");

            if (playerControl.healthPoint <= 0)
            {
                playerControl.animator.SetTrigger("dead");
            }
        }
    }

   

}
