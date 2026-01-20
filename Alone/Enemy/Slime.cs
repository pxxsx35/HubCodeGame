using UnityEngine;
using System.Collections.Generic;

public class Slime : EnemyBase
{
    private HashSet<IDamageable> targetsInRange = new HashSet<IDamageable>();
    playerControl playerScript;
    private void Awake()
    {
        base.Awake();
        playerScript = FindAnyObjectByType<playerControl>();

    }

    protected override void PerformAttack()
    {
        targetsInRange.RemoveWhere(t => t == null || (t as MonoBehaviour) == null);

        foreach (var target in targetsInRange)
        {
            if (target != null)
            {
                target.TakeDamage(damageAmount);
                playerScript.speed /= 2;

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
            playerScript.speed = playerScript.maxspeed;

            targetsInRange.Remove(target);
        }
    }
}