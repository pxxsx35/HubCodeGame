using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerControl : MonoBehaviour , IDamageable
{
    Rigidbody2D rb;
    public Animator animator;
    [SerializeField] float jumpForce, jumpCount;
    public float maxspeed, speed;
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

    private SpriteRenderer sr;
    private Coroutine flashCoroutine;
    private Coroutine poisonCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        quset = FindAnyObjectByType<Quest>();
        allowMove = true;
        sr = GetComponent<SpriteRenderer>();
        speed = maxspeed;
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
            animator.SetTrigger("dead");
            allowMove = false;
          
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

            animator.SetBool("Run", true);  
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


    public void RestartGame()
    {
        audioSource.Stop();
        deadSound.Play();
        animator.enabled = false;
        Debug.Log("Dead!");
        Time.timeScale = 0;
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

    public void ApplyPoison(float totalDamage, float duration)
    {
        if (poisonCoroutine != null) StopCoroutine(poisonCoroutine);
        poisonCoroutine = StartCoroutine(PoisonRoutine(totalDamage, duration));
    }

    private IEnumerator PoisonRoutine(float baseDamage, float duration)
    {
        float timer = duration;
        float tickInterval = 1.0f; // โดนดาเมจทุก 1 วินาที

        while (timer > 0)
        {
            // คำนวณดาเมจให้ลดลงเรื่อยๆ ตามเวลาที่เหลือ
            float currentTickDamage = baseDamage * (timer / duration);

            healthPoint -= currentTickDamage;
            Debug.Log($"[Poison] Tick Damage: {currentTickDamage:F1} | Health Left: {healthPoint:F1}");

            timer -= tickInterval;
            StartCoroutine(HitFlashRoutine());
            yield return new WaitForSeconds(tickInterval);
        }
        poisonCoroutine = null;
    }
    private IEnumerator HitFlashRoutine()
    {

        sr.color = Color.red; // เปลี่ยนเป็นสีแดง

        yield return new WaitForSeconds(0.5f); // รอ 0.5 วินาที

        sr.color = Color.white; // กลับเป็นสีขาว (สีปกติของ Sprite)

        flashCoroutine = null;
    }


    public void TakeDamage(float damageAmount)
    {
        healthPoint -= damageAmount;
        healthPoint = Mathf.Clamp(healthPoint, 0, 100);

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        
        flashCoroutine = StartCoroutine(HitFlashRoutine());
    }


}
