using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyBase
{

    private HashSet<IDamageable> targetsInRange = new HashSet<IDamageable>();
    playerControl playerScript;
    private void Awake()
    {
        base.Awake();
        playerScript = FindAnyObjectByType<playerControl>();

    }

    public override void MoveToPlayer()
    {
        if (CanAttackPlayer) return;
        ResetCombo();
       Vector2 direction = (player.position - transform.position).normalized;


        rig.velocity = direction * moveSpeed;

        float dirX = direction.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(dirX, 1, 1) * 20; 

        IsMoving = true; 
        anim.SetMove(true); 
    }

    protected override void PerformAttack()
    {
        targetsInRange.RemoveWhere(t => t == null || (t as MonoBehaviour) == null);

        foreach (var target in targetsInRange)
        {
            if (target != null)
            {
                target.TakeDamage(damageAmount);

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
