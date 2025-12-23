using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerControl : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    [SerializeField] float speed, jumpForce, jumpCount;
    public float healthPoint = 100; 
    float percentHP;
    public bool allowMove;
    public bool dead;
    [SerializeField] GameObject restart;
    public Image healthBar;
    public Image delayBar;
    float percentDelay;
    Quest quset;
    public AudioSource audioSource;
    public AudioClip walkSound;
    private float previousHealthPoint = 100;

   
    public AudioSource heartBeatAudio; 
    public AudioSource deadSound;

    public AudioSource hurt; 
    public float enemyDetectionRange = 50f; 
    public Transform enemyTransform; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        quset = FindAnyObjectByType<Quest>();
        allowMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();

        if (!allowMove)
        {
            StopMovement();
            return; 
        }

        Move();

        CheckPlayerStatus();

        DetectEnemyAndPlayHeartbeat();

        HandleDamageFeedback();

        DetectEnemyAndPlayHeartbeat();

     
    }

    private void UpdateHealthUI()
    {
    
        percentHP = Mathf.Lerp(percentHP, healthPoint / 100f, Time.deltaTime * 5f);
        percentDelay = Mathf.Lerp(percentDelay, healthPoint / 100f, Time.deltaTime * 2.5f);

        healthBar.fillAmount = percentHP;
        delayBar.fillAmount = percentDelay;
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

      
    

    private void CheckPlayerStatus()
    {
        if (healthPoint <= 0 && !dead)
        {
            dead = true;
            allowMove = false;
            StopAnim(); 
        }
    }

    private void HandleDamageFeedback()
    {
        if (healthPoint != previousHealthPoint)
        {
            hurt.Play();
            previousHealthPoint = healthPoint;
        }
    }

    void Move()
    {
        if (rb.velocity.x != 0 && jumpCount == 2)
        {
     
            if (!audioSource.isPlaying)
            {
                audioSource.GetComponent<AudioSource>().PlayOneShot(walkSound);  
            }

            animator.SetBool("isWalking", true);  
        }
        else
        {
         
            if (audioSource.isPlaying || jumpCount != 2)
            {
                audioSource.Stop(); 
            }
        }

        float horizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(horizontal));

        if (horizontal > 0.1)
        {
            transform.localScale = Vector3.one;
        }
        if (horizontal < -0.1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpCount != 0)
        {
            jumpCount -= 1;
            animator.SetTrigger("jump");
            Debug.Log("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("win"))
        {
            SceneManager.LoadScene("win");
        }
        if (collision.gameObject.CompareTag("key"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().color == new Color(r: 0.5f, g: 0.5f, b: 0.5f))
            {
                Debug.Log("Complete Quest!!");
                quset.CompleteQuest();
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public void StopAnim()
    {
        audioSource.Stop();
        deadSound.Play();
        animator.enabled = false;
        RestartGame();
        Debug.Log("Dead!");
        Time.timeScale = 0;
    }

    void RestartGame()
    {
        restart.SetActive(true);
    }


    

    void DetectEnemyAndPlayHeartbeat()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject enemy in enemies)
        {

            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= enemyDetectionRange)
            {
                if (!heartBeatAudio.isPlaying)
                {
                    heartBeatAudio.Play(); 
                    
                }
                return; 
            }
        }

   
        if (heartBeatAudio.isPlaying)
        {
            heartBeatAudio.Stop();
          
        }
    }

}
