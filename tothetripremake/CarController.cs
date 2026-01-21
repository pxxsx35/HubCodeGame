using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 5f , turbo = 5f , turboTotal = 0 , maxTurbo = 20;
    private float currentSpeed;
    private float interval;
    public float tiltAmount = 15f; // Í§ÈÒ·Õè¨ÐàÍÕÂ§
    public float tiltSpeed = 5f;   // ¤ÇÒÁàÃçÇã¹¡ÒÃàÍÕÂ§
    public float sideSpeed;
    float targetTilt = 0f;
    [Range(0f,1f)]
    public float intensity;

    [SerializeField] GameObject glassBreakScreen;
    [SerializeField] AnimationCurve chromaticCruve;
    [SerializeField] AnimationCurve lensDistortionCruve;
    float timeAnimationCount;
    
    [SerializeField]GameObject carModel;
    [SerializeField] GameObject effModel;
    [SerializeField] TrailRenderer lTrail, rTrail;
    [SerializeField] ParticleSystem lEff, rEff, hitEff;
    [SerializeField] Volume volume;
    [SerializeField] Image healthBar;
    [SerializeField] Image turboBar;
    //public Image turboBar;
    [SerializeField] bool isTurbo , isSlow;
    public GameObject[] targets;
    ChangeStatus npc;


    [SerializeField]private float maxHealth;
    private float currentHealth;

    void Start()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        currentSpeed = speed;
        rb = GetComponent<Rigidbody>();
        npc = FindAnyObjectByType<ChangeStatus>();
       
    }

 
    private void FixedUpdate()
    {
        Move();



        if (turbo > maxTurbo)
            turbo = maxTurbo;

        
    }

    private void Update()
    {

        // --- โค้ดเดิมของคุณ ---
        if (!isTurbo)
        {
            if (timeAnimationCount > 0) timeAnimationCount -= 1 * Time.deltaTime;
        }
        else
        {
            if (timeAnimationCount < 0.30) timeAnimationCount += 1 * Time.deltaTime;
        }

        // --- ส่วนที่เพิ่มเข้าไปใหม่เพื่อจัดการ UI ---
        if (healthBar != null)
        {
            // อัปเดตแถบเลือด (currentHealth / maxHealth)
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (turboBar != null)
        {
            // อัปเดตแถบเทอร์โบ (turboTotal / maxTurbo)
            turboBar.fillAmount = turboTotal / maxTurbo;
        }
        // ---------------------------------------

        // --- โค้ดระบบ Slow เดิมของคุณ ---
        if (isSlow)
        {
            interval += 0.001f * Time.deltaTime;
            float newSpeed = Mathf.Lerp(currentSpeed, speed, interval);
            currentSpeed = newSpeed;
            if (currentSpeed >= speed) isSlow = false;
        }
    }

    public void Move()
    {
       
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(0, 0, currentSpeed * -sideSpeed);
            targetTilt = tiltAmount; 
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(0, 0, currentSpeed * sideSpeed);
            targetTilt = -tiltAmount;
        }
        else
        {
            targetTilt = 0f; 
        }

        if (Input.GetKey(KeyCode.LeftShift) && turboTotal > 0)
        {
            rb.velocity = new Vector3(1 * (currentSpeed + turbo), -10, 0);
            turboTotal -= Time.deltaTime * 4;
            isTurbo = true;

            // effect
            lEff.Play();
            rEff.Play();
            // zoom screen
            if (volume.profile.TryGet(out ChromaticAberration chromatic))
            {
                float chromaticIntensity = chromaticCruve.Evaluate(timeAnimationCount);
                chromatic.intensity.value = chromaticIntensity;

            }
            if (volume.profile.TryGet(out LensDistortion lens))
            {
                float lensIntensity = lensDistortionCruve.Evaluate(timeAnimationCount);
                lens.intensity.value = lensIntensity;
            }
        }
        else
        {
            isTurbo = false;
            
            rb.velocity = new Vector3(1 * currentSpeed, -10, 0);
            // effect
            lEff.Stop();
            rEff.Stop();
            // zoom screen
            if (volume.profile.TryGet(out ChromaticAberration chromatic))
            {
                float chromaticIntensity = chromaticCruve.Evaluate(timeAnimationCount);
                chromatic.intensity.value = chromaticIntensity;

            }
            if (volume.profile.TryGet(out LensDistortion lens))
            {
                float lensIntensity = lensDistortionCruve.Evaluate(timeAnimationCount);
                lens.intensity.value = lensIntensity;
            }
        }
        //turboBar.fillAmount = turboTotal / maxTurbo;



        //¤Çº¤ØÁ¡ÒÃàÍÕÂ§¢Í§Ã¶µÍ¹àÅÕéÂÇ ¤èÒ¡ÒÃËÁØ¹ÁÒ¨Ò¡¢éÒ§º¹¹Õé¹Ð ^^^
        Quaternion targetRotation = Quaternion.Euler(0f, targetTilt, 0f);
        carModel.transform.rotation = Quaternion.Lerp(carModel.transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
    }

    public void takeDamage(float amount)
    {
        glassBreakScreen.SetActive(false);
        glassBreakScreen.SetActive(true);
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Debug.Log("player Die");
            //Time.timeScale = 0f;
            SceneManager.LoadScene("SampleScene");

        }
        
    }

    public void heal()
    {
        currentHealth += 0.25f;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void turborAdd()
    {
        if (!isTurbo)
        {
            if (turboTotal < maxTurbo)
            {
                turboTotal += 5; 
                turboAddEffect();
            }
        }
    }

    public void turboAddEffect()
    { 
        effModel.SetActive(true);
        Invoke("turboRemoveEffect", 0.5f);
    }
    void turboRemoveEffect()
    {
        effModel.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Npc"))
        {
            if(isTurbo == false)
            {
                currentSpeed -= (currentSpeed / 4);
                interval = 0;
                isSlow = true;
                if (collision.gameObject.TryGetComponent(out carBot enemyCar))
                {
                    hitEff.Play();
                    enemyCar.BounceOff();
                    takeDamage(1);
                }
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out carBot enemyCar))
                {
                    enemyCar.Dead();
                    heal();
                }
            }
            
        }

        if(collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("SampleScene");

        }


    }

 
}
