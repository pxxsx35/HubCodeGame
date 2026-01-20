using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : EnemyBase
{
    [Header("Attack Settings")]
    private HashSet<IDamageable> targetsInRange = new HashSet<IDamageable>();
    playerControl playerScript;
    [Header("Attack Settings")]
    [SerializeField] private float initialDamage = 2f; // ดาเมจตอนกัดทันที
    [SerializeField] private float totalPoisonDamage = 10f; // ดาเมจพิษรวม
    [SerializeField] private float poisonDuration = 2.5f; // ระยะเวลาติดพิษ
    private void Awake()
    {
        base.Awake();
        playerScript = FindAnyObjectByType<playerControl>();

    }

 

    protected override void PerformAttack()
    {
        // เคลียร์ Object ที่พังไปแล้ว
        targetsInRange.RemoveWhere(t => t == null || (t as MonoBehaviour) == null);

        foreach (var target in targetsInRange)
        {
            if (target != null)
            {
                // 1. ทำดาเมจจากการกัดปกติก่อน
                target.TakeDamage(initialDamage);

                // 2. ส่งสถานะพิษไปที่เป้าหมาย
                target.ApplyPoison(totalPoisonDamage, poisonDuration);

                Debug.Log("[Rat] Bite and Poisoned target!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            targetsInRange.Add(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {

            targetsInRange.Remove(target);
        }
    }
}
