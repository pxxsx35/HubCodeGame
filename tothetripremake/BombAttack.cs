using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject insideCircle, outsideCircle;
    [SerializeField] ParticleSystem eff;
    [SerializeField] LayerMask player;
    [SerializeField] Color startColor, endColor;
    [SerializeField] float maxCooldown;
    [SerializeField] float radias;
    float currentCooldown = 0f;
    bool isBomb = false;
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBomb)
        {
            if (currentCooldown < maxCooldown)
            {
                currentCooldown += 1 * Time.deltaTime; // นับถอยหลังเวลา
                float coolTime = currentCooldown / maxCooldown;
                Color newColor = Color.Lerp(startColor, endColor, coolTime);
                // ขยายวงภายในให้เต็ม
                insideCircle.transform.localScale = new Vector3(coolTime, coolTime, coolTime);
                // เปลี่ยนสีวง ตามเวลา
                insideCircle.transform.GetComponentInChildren<Renderer>().material.color = newColor;
                outsideCircle.GetComponent<Renderer>().material.color = newColor;
            }
            else
            {
                // แสดง effect
                eff.Play();
                // ลบวงกลม
                Destroy(insideCircle);
                Destroy(outsideCircle);
                // เช็คว่าผู้เล่นอยู่ในระยะ ที่โดนดาเมจไหม
                Collider[] collierHit = Physics.OverlapSphere(transform.position, radias, player);
                foreach (Collider col in collierHit)
                {
                    if (col.TryGetComponent(out CarController car))
                    {
                        Debug.Log("player hits");
                        car.takeDamage(1f);
                    }
                    else
                    {
                        Debug.Log("nothing here");
                    }
                }

                isBomb = true;
                Destroy(gameObject, 0.75f);
            }
        }

        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radias);
    }
}
